using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Data.Database;
using Server.Data.Entities;
using Server.Data.Repositories;
using Server.Infrastructure.Models;
using Server.Infrastructure.Services;

namespace Server.Infrastructure.Common
{
    public class Country
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }

    public class Url
    {
        public string Original { get; set; }
        public string Alias { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public static class SeedData
    {
        public static void Seed(ApplicationDbContext dbContext, IPasswordsService passwordsService)
        {
            Country[] countries =
            {
                new Country
                {
                    CountryName = "Russia",
                    CountryCode = "RU"
                },
                new Country
                {
                    CountryName = "Finland",
                    CountryCode = "FI"
                },
                new Country
                {
                    CountryName = "Germany",
                    CountryCode = "GE"
                },
                new Country
                {
                    CountryName = "France",
                    CountryCode = "FR"
                },
                new Country
                {
                    CountryName = "Spain",
                    CountryCode = "SP"
                }
            };

            string[] browsers =
            {
                "Google Chrome",
                "Firefox",
                "Safari",
                "Internet Explorer",
                "Edge",
                "Opera"
            };

            string[] platforms =
            {
                "Windows",
                "MacOS",
                "Android",
                "Linux"
            };

            dbContext.Database.EnsureCreated();

            if (!dbContext.Users.Any())
            {
                dbContext.Users.Add(new User
                {
                    Email = "admin@example.com",
                    Role = Role.Admin.ToString(),
                    PasswordHash = passwordsService.Hash("123456"),
                    EmailVerifiedAt = DateTime.UtcNow
                });

                dbContext.SaveChanges();
            }

            User adminUser = dbContext.Users.First(u=>u.Role == Role.Admin.ToString());

            Url[] urls =
            {
                new Url
                {
                    Original = "https://github.com",
                    Alias = "github",
                    ExpiresAt = DateTime.UtcNow.AddDays(10)
                },
                new Url
                {
                    Original = "https://www.google.co.uk",
                    Alias = "Xc8zja"
                },
                new Url()
                {
                    Original = "https://www.youtube.com",
                    Alias = "Kix90bk",
                    ExpiresAt = DateTime.UtcNow.AddHours(-10)
                }
            };

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

                    for (int i = 0; i < 100; i++)
                    {
                        Country country = countries[r.Next(0, countriesLength - 1)];
                        string browser = browsers[r.Next(0, browsersLength - 1)];
                        string platform = platforms[r.Next(0, platformsLength - 1)];

                        navigations.Add(new Navigation
                        {
                            CountryCode = country.CountryCode,
                            CountryName = country.CountryName,
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
