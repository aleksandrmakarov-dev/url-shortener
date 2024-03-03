using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Server.API.Common;
using Server.API.Middlewares;
using Server.Data.Database;
using Server.Infrastructure.Common;
using Server.Infrastructure.Interfaces;

namespace Server.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // add services to the container.

            // add db context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite("Data Source=mydb.db");
            });

            // add automapper
            builder.Services.AddAutoMapper(
                Assembly.GetExecutingAssembly(),
                Assembly.Load("Server.Infrastructure")
                );

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });

            builder.Services.AddHttpClient<ILocationService>();

            // add repositories
            builder.Services.AddRepositories();

            // add services
            builder.Services.AddServices();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // ignore omitted parameters on models to enable optional params (e.g. User update)
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.Configure<ForwardedHeadersOptions>(options => {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection();

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.MapControllers();

            app.UseAuthorization();

            // global exception handler middleware
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            
            // user extractor middleware
            app.UseMiddleware<UserExtractorMiddleware>();

            using (var scope = app.Services.CreateScope())
            {
                SeedData.Seed(
                    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(),
                    scope.ServiceProvider.GetRequiredService<IPasswordsService>()
                );
            }

            app.Run();
        }
    }
}
