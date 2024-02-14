using Microsoft.EntityFrameworkCore;
using Server.Csharp.Data.Models;

namespace Server.Csharp.Data.Database
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<ShortUrl> ShortUrl { get; set; }
        public DbSet<User> Users { get; set; }  
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Role> Roles { get; set; } 

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
