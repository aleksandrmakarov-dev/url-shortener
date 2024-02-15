using Server.Csharp.Business.Options;
using Server.Csharp.Business.Services;

namespace Server.Csharp.Business.Common
{
    public static class Register
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, EfAuthService>();
            services.AddScoped<IUsersService, EfUsersService>();
            services.AddScoped<ISessionsService, EfSessionsService>();
            services.AddScoped<IShortUrlsService, EfShortUrlsService>();

            services.AddScoped<IEmailService, MockEmailService>();
            
            services.AddSingleton<IPasswordService, BcryptService>();
            services.AddSingleton<ITokenService, TokenService>();

            return services;
        }

        public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection(JwtOptions.SectionName)
            );

            return builder;
        }
    }
}
