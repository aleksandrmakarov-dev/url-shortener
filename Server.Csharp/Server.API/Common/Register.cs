using Server.Data.Entities;
using Server.Data.Repositories;
using Server.Infrastructure.Services;

namespace Server.API.Common
{
    public static class Register
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ISessionsRepository, SessionsRepository>();
            services.AddScoped<IShortUrlsRepository, ShortUrlsRepository>();
            services.AddScoped<INavigationsRepository, NavigationsRepository>();

            //cache repositories
            services.AddScoped<IGenericCacheRepository<ShortUrl>, GenericCacheRepository<ShortUrl>>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IShortUrlsService, ShortUrlsService>();
            services.AddScoped<INavigationsService,NavigationsService>();
            services.AddScoped<IStatisticsService, StatisticsService>();

            services.AddScoped<IPasswordsService, BcryptPasswordsService>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
