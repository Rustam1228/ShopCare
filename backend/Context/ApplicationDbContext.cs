using backend.Context.Configuration;
using backend.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace backend.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options)
    {
        public DbSet<Care> Cares { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.ApplyConfiguration(new ConfigurationImage());
            modelBuilder.ApplyConfiguration(new ConfigurationUser());

            base.OnModelCreating(modelBuilder);
        }
    }
}
