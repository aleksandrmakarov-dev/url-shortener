using AutoMapper;
using Server.Data.Entities;
using Server.Data.Repositories;
using Server.Infrastructure.Exceptions;
using Server.Infrastructure.Models;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ISessionsRepository _sessionsRepository;

    private readonly IPasswordsService _passwordsService;
    private readonly ITokensService _tokensService;
    private readonly IJwtService _jwtService;

    private readonly IMapper _mapper;

    public AuthService(
        IUsersRepository usersRepository, 
        IPasswordsService passwordsService, 
        IMapper mapper, 
        ISessionsRepository sessionsRepository, 
        ITokensService tokensService, 
        IJwtService jwtService)
    {
        _usersRepository = usersRepository;
        _passwordsService = passwordsService;
        _mapper = mapper;
        _sessionsRepository = sessionsRepository;
        _tokensService = tokensService;
        _jwtService = jwtService;
    }

    public async Task<EmailVerificationResponse> SignUpAsync(SignUpRequest request)
    {
        // check if email is already registered in database
        User? foundUser = await _usersRepository.GetByEmailAsync(request.Email);

        // if user is found throw and exception that user is already registered
        if (foundUser != null)
        {
            throw new BadRequestException($"Email address {request.Email} is already registered");
        }

        // otherwise map request to User model
        User userToCreate = _mapper.Map<SignUpRequest, User>(request);
        
        // generate password hash from request password
        userToCreate.PasswordHash = _passwordsService.Hash(request.Password);
        userToCreate.Role = Role.User.ToString();

        // generate email verification token
        string emailVerificationToken = _tokensService.GetToken(128);
        
        userToCreate.EmailVerificationToken = emailVerificationToken;
        userToCreate.EmailVerificationTokenExpiresAt = DateTime.UtcNow.AddHours(12);

        // write user to the database
        User createdUser =  await _usersRepository.CreateAsync(userToCreate);

        //map User to SignUpResponse and return it
        return _mapper.Map<User, EmailVerificationResponse>(createdUser);
    }

    public async Task<SignInResponse> SignInAsync(SignInRequest request)
    {
        User? foundUser = await _usersRepository.GetByEmailAsync(request.Email);
        
        if (foundUser == null)
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        // implement user is locked out feature

        // check if email is verified

        if (foundUser.EmailVerifiedAt == null)
        {
            // if not throw exception
            throw new BadRequestException("Email is not verified. User can sign in to account only after email verification");
        }

        // generate session refresh token

        string refreshToken = _tokensService.GetToken(128);

        // check if token exists in database
        Session? foundSession = await _sessionsRepository.GetByRefreshTokenAsync(refreshToken);

        // if token found generate new token
        if (foundSession !=null)
        {
            refreshToken = _tokensService.GetToken(128);
        }

        // create session
        Session sessionToCreate = new Session
        {
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = foundUser.Id
        };

        // write session to database
        Session createdSession = await _sessionsRepository.CreateAsync(sessionToCreate);

        // generate access token
        string accessToken = _jwtService.GetToken(new JwtPayload
        {
            Id = createdSession.UserId,
            Role = Enum.Parse<Role>(foundUser.Role)
        });
        
        return new SignInResponse
        {
            RefreshToken = createdSession.RefreshToken,
            Session = new SessionResponse
            {
                AccessToken = accessToken,
                UserId = createdSession.UserId,
                Email = foundUser.Email,
                Role = Enum.Parse<Role>(foundUser.Role)
            }
        };
    }

    public async Task VerifyEmailAsync(VerifyEmailRequest request)
    {
        // check if user with email and token exists

        User? foundUser = await _usersRepository.GetByEmailAsync(request.Email);

        if (foundUser == null || foundUser.EmailVerificationToken != request.Token)
        {
            throw new UnauthorizedException("Invalid email or token");
        }

        if (foundUser.EmailVerifiedAt != null)
        {
            return;
        }

        // if token is expired throw an error
        if (foundUser.EmailVerificationTokenExpiresAt != null &&  foundUser.EmailVerificationTokenExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Email verification token is expired");
        }

        // set email verification token and  datetime when it expires to null and set email verified at to now
        foundUser.EmailVerificationToken = null;
        foundUser.EmailVerificationTokenExpiresAt = null;
        foundUser.EmailVerifiedAt = DateTime.UtcNow;

        // update user
        await _usersRepository.UpdateAsync(foundUser);
    }

    public async Task<SessionResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // check if session exists
        Session? foundSession = await _sessionsRepository.GetByRefreshTokenAsync(request.Token);

        // if session not found throw an error
        if (foundSession == null)
        {
            throw new UnauthorizedException("Invalid refresh token");
        }

        // if session is expired throw an error
        if (foundSession.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token is expired");
        }

        // get user by session userId

        User? foundUser = await _usersRepository.GetByIdAsync(foundSession.UserId);

        // if user not found throw an error
        if (foundUser == null)
        {
            throw new UnauthorizedException("No user associated with session");
        }

        // generate new access token
        string accessToken = _jwtService.GetToken(new JwtPayload
        {
            Id = foundUser.Id,
            Role = Enum.Parse<Role>(foundUser.Role)
        });

        // return access token and user id
        return new SessionResponse
        {
            AccessToken = accessToken,
            UserId = foundUser.Id,
            Email = foundUser.Email,
            Role = Enum.Parse<Role>(foundUser.Role)
        };
    }

    public async Task<EmailVerificationResponse?> NewEmailVerificationAsync(NewEmailVerificationRequest request)
    {
        // check if user with email exists
        User? foundUser = await _usersRepository.GetByEmailAsync(request.Email);

        // if user not found throw an error
        if (foundUser == null)
        {
            throw new NotFoundException("User is not registered");
        }

        // if user email is already verified return null
        if (foundUser.EmailVerifiedAt != null)
        {
            return null;
        }

        // generate new email verification token
        string emailVerificationToken = _tokensService.GetToken(128);

        // set new token and expiration date to user
        foundUser.EmailVerificationToken = emailVerificationToken;
        foundUser.EmailVerificationTokenExpiresAt = DateTime.UtcNow.AddHours(12);

        // update user
        User updatedUser = await _usersRepository.UpdateAsync(foundUser);

        //map User to SignUpResponse and return it
        return _mapper.Map<User, EmailVerificationResponse>(updatedUser);
    }

    public async Task SignOutAsync(SignOutRequest request)
    {
        // check if session exists
        Session? foundSession = await _sessionsRepository.GetByRefreshTokenAsync(request.Token);

        // if session not found throw an error
        if (foundSession == null)
        {
            throw new UnauthorizedException("Invalid refresh token");
        }

        // if session is expired return
        if (foundSession.ExpiresAt < DateTime.UtcNow)
        {
            return;
        }

        // set new expiration time datetime now
        foundSession.ExpiresAt = DateTime.UtcNow;
        
        // update date in database
        await _sessionsRepository.UpdateAsync(foundSession);
    }
}