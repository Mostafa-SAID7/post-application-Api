using Microsoft.EntityFrameworkCore;
using Post.Infrastructure.Persistence.Configurations;

namespace Post.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post.Domain.Entities.Post> Posts { get; set; }
        public DbSet<Post.Domain.Entities.Category> Categories { get; set; }
        public DbSet<Post.Domain.Entities.Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations from separate files
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
        }
    }
}

