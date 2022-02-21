using Microsoft.EntityFrameworkCore;
using PlatformService.Model;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
             
        }
        

        #region Commented code 
        //public DbSet<User> users { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            
            builder.Entity<Platform>()
                .HasMany(p => p.Commands)
                .WithOne(p => p.Platform!)
                .HasForeignKey(p => p.PlatformId);
            
            builder.Entity<Command>()
                .HasOne(p => p.Platform)
                .WithMany(p => p.Commands)
                .HasForeignKey(p => p.PlatformId);
            
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}