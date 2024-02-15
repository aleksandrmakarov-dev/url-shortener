using Server.Csharp.Business.Common;
using Server.Csharp.Data.Entities;
using Server.Csharp.Data.Repositories;

namespace Server.Csharp.Business.Services
{
    public class EfSessionsService:ISessionsService
    {
        private readonly ISessionsRepository _sessionsRepository;
        private readonly ITokenService _tokenService;

        public EfSessionsService(ISessionsRepository sessionsRepository, ITokenService tokenService)
        {
            _sessionsRepository = sessionsRepository;
            _tokenService = tokenService;
        }

        public async Task<Session> CreateAsync(Guid userId)
        {
            string refreshToken = _tokenService.GetToken(256);

            Session sessionToCreate = new Session
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(Constants.RefreshTokenExpires),
                UserId = userId
            };

            Session createdSession = await _sessionsRepository.CreateAsync(sessionToCreate);

            return createdSession;
        }
    }
}
