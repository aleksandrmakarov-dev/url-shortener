using Server.Data.Database;
using Server.Data.Entities;
using Server.Infrastructure.Interfaces;
using Server.Infrastructure.Models;

namespace Server.Infrastructure.Common
{
    public class Country
    {
        public string CountryName { get; set; }
    }

    public class Url
    {
        public string Original { get; set; }
        public string Alias { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public static class SeedData
    {
        public static void Seed(ApplicationDbContext dbContext, IPasswordsService passwordsService,ITokensService tokenService)
        {
            string[] countries =
            [
                "Russia",
                "Finland",
                "Germany",
                "France",
                "Spain"
            ];

            string[] browsers =
            [
                "Google Chrome",
                "Firefox",
                "Safari",
                "Internet Explorer",
                "Edge",
                "Opera"
            ];

            string[] platforms =
            [
                "Windows",
                "MacOS",
                "Android",
                "Linux"
            ];

            dbContext.Database.EnsureCreated();

            if (!dbContext.Users.Any())
            {
                dbContext.Users.AddRange([new User
                    {
                        Email = "admin@example.com",
                        Role = Role.Admin.ToString(),
                        PasswordHash = passwordsService.Hash("admin123"),
                        EmailVerifiedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Email = "user@example.com",
                        Role = Role.User.ToString(),
                        PasswordHash = passwordsService.Hash("user123"),
                        EmailVerifiedAt = DateTime.UtcNow
                    }
                ]);

                dbContext.SaveChanges();
            }

            User adminUser = dbContext.Users.First(u=>u.Role == Role.Admin.ToString());
            User regularUser = dbContext.Users.First(u => u.Role == Role.User.ToString());

            Url[] urls =
            [
                new()
                {
                    Original = "https://github.com",
                    Alias = tokenService.GetToken(8),
                    ExpiresAt = DateTime.UtcNow.AddDays(10)
                },
                new()
                {
                    Original = "https://www.google.co.uk",
                    Alias = "google"
                },
                new()
                {
                    Original = "https://www.youtube.com",
                    Alias = "youtube",
                    ExpiresAt = DateTime.UtcNow.AddHours(-10)
                }
            ];

            Url[] urls2 =
            [
                new()
                {
                    Original = "https://github.com",
                    Alias = "github",
                    ExpiresAt = DateTime.UtcNow.AddDays(10)
                },
                new()
                {
                    Original = "https://www.google.co.uk",
                    Alias = tokenService.GetToken(8)
                },
                new()
                {
                    Original = "https://www.youtube.com",
                    Alias = tokenService.GetToken(8),
                    ExpiresAt = DateTime.UtcNow.AddHours(-20)
                }
            ];

            if (!dbContext.ShortUrls.Any())
            {
                foreach (var item in urls)
                {
                    dbContext.ShortUrls.Add(new ShortUrl
                    {
                        Original = item.Original,
                        Alias = item.Alias,
                        ExpiresAt = item.ExpiresAt,
                        UserId = adminUser.Id
                    });
                }

                foreach (var item in urls2)
                {
                    dbContext.ShortUrls.Add(new ShortUrl
                    {
                        Original = item.Original,
                        Alias = item.Alias,
                        ExpiresAt = item.ExpiresAt,
                        UserId = regularUser.Id
                    });
                }

                dbContext.SaveChanges();
            }

            if (!dbContext.Navigations.Any())
            {
                var foundUrls = dbContext.ShortUrls.ToList();

                foreach (var url in foundUrls)
                {
                    List<Navigation> navigations = new List<Navigation>();

                    Random r = new Random(DateTime.Now.Millisecond);

                    int countriesLength = countries.Length;
                    int browsersLength = browsers.Length;
                    int platformsLength = platforms.Length;

                    for (int i = 0; i < r.Next(50,100); i++)
                    {
                        string country = countries[r.Next(0, countriesLength)];
                        string browser = browsers[r.Next(0, browsersLength)];
                        string platform = platforms[r.Next(0, platformsLength)];

                        navigations.Add(new Navigation
                        {
                            Country = country,
                            Browser = browser,
                            Platform = platform,
                            IpAddress = $"{r.Next(0, 255)}.{r.Next(0, 255)}.{r.Next(0, 255)}.{r.Next(0, 255)}",
                            ShortUrlId = url.Id
                        });
                    }

                    dbContext.Navigations.AddRange(navigations);
                }

                dbContext.SaveChanges();
            }
        }
    }
}
