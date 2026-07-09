using Microsoft.EntityFrameworkCore;

namespace Post.Infrastructure.Persistence.Seeds
{
    /// <summary>
    /// Extension methods for database migrations and seeding
    /// Called from the API layer during application startup
    /// </summary>
    public static class MigrationExtensions
    {
        /// <summary>
        /// Apply pending migrations and seed database with initial data
        /// </summary>
        public static async Task ApplyMigrationsAndSeedAsync(this AppDbContext dbContext)
        {
            try
            {
                // Apply pending migrations
                await dbContext.Database.MigrateAsync();

                // Seed initial data by calling seeders in order
                await SeedDataAsync(dbContext);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while applying migrations or seeding database.", ex);
            }
        }

        private static async Task SeedDataAsync(AppDbContext context)
        {
            // Check if database already has data
            if (await context.Categories.AnyAsync() || await context.Tags.AnyAsync() || await context.Posts.AnyAsync())
            {
                return;
            }

            // Seed categories
            var categories = CategorySeeder.GenerateCategories();
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Seed tags
            var tags = TagSeeder.GenerateTags();
            await context.Tags.AddRangeAsync(tags);
            await context.SaveChangesAsync();

            // Seed posts (depends on categories and tags)
            var retrievedCategories = await context.Categories.ToListAsync();
            var retrievedTags = await context.Tags.ToListAsync();
            var posts = PostSeeder.GeneratePosts(retrievedCategories, retrievedTags);
            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();
        }
    }
}
