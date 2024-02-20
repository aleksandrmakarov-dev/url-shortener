
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Server.API.Common;
using Server.Data.Database;

namespace Server.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite("Data Source=mydb.db");
            });

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly(),Assembly.LoadFile("Server.Data"),
                Assembly.LoadFile("Server.Infrastructure"));

            builder.Services.AddRepositories();

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

            app.UseHttpsRedirection();

            // app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
