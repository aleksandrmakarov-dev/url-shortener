
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Server.Csharp.Business.Common;
using Server.Csharp.Business.Services;
using Server.Csharp.Data.Common;
using Server.Csharp.Data.Database;
using Server.Csharp.Presentation.Middlewares;


namespace Server.Csharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var policyName = "AllowOriginsPolicy";

            var builder = WebApplication.CreateBuilder(args);

            // Add options
            builder.AddOptions();

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite("Data Source=database.db;");
            });

            builder.Services.AddRepositories();
            builder.Services.AddServices();

            // Add automapper

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name:policyName, policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExtractUserMiddleware>();
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                SeedData.Seed(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(),
                    scope.ServiceProvider.GetRequiredService<IPasswordService>());
            }

            app.Run();
        }
    }
}
