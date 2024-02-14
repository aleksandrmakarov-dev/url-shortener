using Server.Csharp.Data.Repositories;

namespace Server.Csharp.Data.Common
{
    public static class Register
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }
    }
}
