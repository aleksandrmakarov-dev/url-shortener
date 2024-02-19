using AutoMapper;
using Server.Data.Entities;
using Server.Data.Repositories;
using Server.Infrastructure.Models.Requests;
using Server.Infrastructure.Models.Responses;

namespace Server.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ISessionsRepository _sessionsRepository;

    private readonly IPasswordsService _passwordsService;
    private readonly IMapper _mapper;

    public AuthService(
        IUsersRepository usersRepository, 
        IPasswordsService passwordsService, 
        IMapper mapper, 
        ISessionsRepository sessionsRepository
        )
    {
        _usersRepository = usersRepository;
        _passwordsService = passwordsService;
        _mapper = mapper;
        _sessionsRepository = sessionsRepository;
    }

    public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
    {
        // check if email is already registered in database
        User? foundUser = await _usersRepository.GetByEmailAsync(request.Email);

        // if user is found throw and exception that user is already registered
        if (foundUser != null)
        {
            throw new Exception($"Email address {request.Email} is already registered");
        }

        // otherwise map request to User model
        User userToCreate = _mapper.Map<SignUpRequest, User>(request);

        // generate password hash from request password
        userToCreate.PasswordHash = _passwordsService.Hash(request.Password);

        // generate email verification token

        // write user to the database
        User createdUser =  await _usersRepository.CreateAsync(userToCreate);

        //map User to SignUpResponse and return it
        return _mapper.Map<User, SignUpResponse>(createdUser);
    }

    public async Task<SignInResponse> SignInAsync(SignInRequest request)
    {
        User? foundUser = await _usersRepository.GetByEmailAsync(request.Email);
        
        if (foundUser == null)
        {
            throw new Exception("Invalid email or password");
        }

        // implement user is locked out feature

        // check if email is verified

        if (foundUser.EmailVerifiedAt == null)
        {
            // if not throw exception
            throw new Exception("Email is not verified. User can sign in to account only after email verification");
        }

        // generate session refresh token

        string refreshToken = "";

        Session sessionToCreate = new Session
        {
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = foundUser.Id
        };

        Session createdSession = await _sessionsRepository.CreateAsync(sessionToCreate);

        string accessToken = "";

        return new SignInResponse
        {
            RefreshToken = createdSession.RefreshToken,
            Session = new SessionResponse
            {
                AccessToken = accessToken,
                UserId = createdSession.UserId
            }
        };
    }
}