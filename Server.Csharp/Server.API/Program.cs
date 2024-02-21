
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Server.API.Common;
using Server.API.Middlewares;
using Server.Data.Database;

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

            // add repositories
            builder.Services.AddRepositories();

            // add services
            builder.Services.AddServices();

            builder.Services.AddControllers();
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

            // app.UseAuthorization();


            app.MapControllers();

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<UserExtractorMiddleware>();

            app.Run();
        }
    }
}
