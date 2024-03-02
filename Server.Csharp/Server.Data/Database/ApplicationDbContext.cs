using Microsoft.EntityFrameworkCore;
using Server.Data.Entities;

namespace Server.Data.Database
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<ShortUrl> ShortUrls { get; set; }
        public DbSet<Navigation> Navigations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .HasColumnType("VARCHAR")
                .HasMaxLength(200);

            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique(true);

            modelBuilder.Entity<Session>()
                .HasIndex(e=>e.RefreshToken)
                .IsUnique(true);

            modelBuilder.Entity<ShortUrl>()
                .HasIndex(e=>e.Alias)
                .IsUnique(true);    
        }
    }
}
