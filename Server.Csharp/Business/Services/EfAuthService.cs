using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Server.Csharp.Business.Models.Common;
using Server.Csharp.Business.Models.Requests;
using Server.Csharp.Business.Models.Responses;
using Server.Csharp.Data.Database;
using Server.Csharp.Data.Models;
using Server.Csharp.Data.Repositories;
using Server.Csharp.Presentation.Exceptions;

namespace Server.Csharp.Business.Services
{
    public class EfAuthService:IAuthService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public EfAuthService(
            IMapper mapper, 
            ITokenService tokenService, 
            IEmailService emailService, 
            IUsersRepository usersRepository, 
            IPasswordService passwordService)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _usersRepository = usersRepository;
            _passwordService = passwordService;
        }

        public async Task<EmailVerificationToken> SignUpAsync(SignUpRequest request)
        {
            bool isEmailRegistered = await _usersRepository.IsExistsByEmailAsync(request.Email);

            if (isEmailRegistered)
            {
                throw new ConflictException($"Email '{request.Email}' is already registered");
            }

            User userToCreate = _mapper.Map<SignUpRequest,User>(request);
            userToCreate.PasswordHash = _passwordService.Hash(request.Password);

            EmailVerificationToken emailVerificationToken = new EmailVerificationToken
            {
                Email = userToCreate.Email,
                Value = _tokenService.GetToken(128),
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            userToCreate.EmailVerificationToken = emailVerificationToken.Value;
            userToCreate.EmailVerificationTokenExpiresAt = emailVerificationToken.ExpiresAt;

            await _usersRepository.CreateAsync(userToCreate);

            return emailVerificationToken;
        }

        public async Task<TokenResponse> SignInAsync(SignInRequest request)
        {
            User? foundUser = await _usersRepository.GetByEmailAsync(request.Email);

            if (foundUser == null)
            {
                throw new NotFoundException($"Email '{request.Email}' not found");
            }


            bool isPasswordCorrect = _passwordService.Verify(request.Password, foundUser.PasswordHash);

            if (!isPasswordCorrect)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            if (foundUser.EmailVerifiedAt == null)
            {
                throw new UnauthorizedException("You must verify your email address before signing in.");
            }

            TokenResponse tokenResponse = new TokenResponse
            {
                AccessToken = _tokenService.GetJwtToken(foundUser),
                RefreshToken = _tokenService.GetToken(256)
            };

            return tokenResponse;
        }


        public async Task VerifyEmailAsync(VerifyEmailRequest request)
        {
            User? foundUser = await _usersRepository.GetByEmailAndVerificationTokenAsync(request.Email, request.Token);

            if (foundUser == null)
            {
                throw new NotFoundException($"Invalid email or token");
            }

            foundUser.EmailVerifiedAt = DateTime.Now;
            foundUser.EmailVerificationToken = null;
            foundUser.EmailVerificationTokenExpiresAt = null;

            await _usersRepository.UpdateAsync(foundUser);
        }
    }
}
