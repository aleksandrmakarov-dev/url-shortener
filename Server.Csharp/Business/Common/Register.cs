using Server.Csharp.Business.Services;

namespace Server.Csharp.Business.Common
{
    public static class Register
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, EfAuthService>();
            services.AddScoped<IUsersService, EfUsersService>();


            services.AddScoped<IEmailService, MockEmailService>();
            
            services.AddSingleton<IPasswordService, BcryptService>();
            services.AddSingleton<ITokenService, TokenService>();

            return services;
        }
    }
}
