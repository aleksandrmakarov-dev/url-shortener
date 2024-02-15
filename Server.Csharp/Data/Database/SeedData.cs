using Server.Csharp.Business.Services;
using Server.Csharp.Data.Entities;
using Server.Csharp.Data.Repositories;
using Server.Csharp.Presentation.Common;

namespace Server.Csharp.Data.Database
{
    public static class SeedData
    {
        public static void Seed(
            ApplicationDbContext ctx, 
            IPasswordService passwordService)
        {

            if (!ctx.Roles.Any())
            {
                ctx.Roles.AddRange(new []
                {
                    new Role{Name = Roles.Admin.ToString()},
                    new Role{Name = Roles.User.ToString()}
                });

                ctx.SaveChanges();
            }



            if (!ctx.Users.Any())
            {

                Role adminRole = ctx.Roles.First(r => r.Name == Roles.Admin.ToString());
                Role userRole = ctx.Roles.First(r => r.Name == Roles.User.ToString());

                var users = new User[]
                {
                    new User
                    {
                        Email = "admin@example.com",
                        PasswordHash = passwordService.Hash("admin123"),
                        EmailVerifiedAt = DateTime.UtcNow,
                        RoleId = adminRole.Id
                    },
                    new User
                    {
                        Email = "user@example.com",
                        PasswordHash = passwordService.Hash("user123"),
                        EmailVerifiedAt = DateTime.UtcNow,
                        RoleId = userRole.Id
                    },
                    new User
                    {
                        Email = "alex@example.com",
                        PasswordHash = passwordService.Hash("alex123"),
                        EmailVerifiedAt = DateTime.UtcNow,
                        RoleId = userRole.Id
                    }
                };

                ctx.Users.AddRange(users);
                ctx.SaveChanges();
            }

            if (!ctx.ShortUrls.Any())
            {
                User admin = ctx.Users.First(u => u.Email == "admin@example.com");
                User user = ctx.Users.First(u => u.Email == "user@example.com");
                User alex = ctx.Users.First(u => u.Email == "alex@example.com");

                var urls = new ShortUrl[]
                {
                    new ShortUrl
                    {
                        Original = "https://google.com",
                        UserId = admin.Id,
                    },
                    new ShortUrl
                    {
                        Original = "https://youtube.com",
                        UserId = admin.Id,
                    },
                    new ShortUrl
                    {
                        Original = "https://microsoft.com",
                        UserId = admin.Id,
                    }
                };

                ctx.ShortUrls.AddRange(urls);
                ctx.SaveChanges();
            }
        }
    }
}
