using Microsoft.EntityFrameworkCore;

namespace Post.Infrastructure.Persistence.Seeds
{
    /// <summary>
    /// Orchestrates database seeding by coordinating individual entity seeders
    /// Runs during application startup to populate initial data
    /// </summary>
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            try
            {
                // Check if database already has data
                if (await context.Categories.AnyAsync() || await context.Tags.AnyAsync() || await context.Posts.AnyAsync())
                {
                    return;
                }

                // Seed categories
                await SeedCategoriesAsync(context);

                // Seed tags
                await SeedTagsAsync(context);

                // Seed posts (depends on categories and tags)
                await SeedPostsAsync(context);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while seeding the database.", ex);
            }
        }

        private static async Task SeedCategoriesAsync(AppDbContext context)
        {
            var categories = CategorySeeder.GenerateCategories();
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedTagsAsync(AppDbContext context)
        {
            var tags = TagSeeder.GenerateTags();
            await context.Tags.AddRangeAsync(tags);
            await context.SaveChangesAsync();
        }

        private static async Task SeedPostsAsync(AppDbContext context)
        {
            // Retrieve seeded categories and tags
            var categories = await context.Categories.ToListAsync();
            var tags = await context.Tags.ToListAsync();

            var posts = PostSeeder.GeneratePosts(categories, tags);
            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();
        }
    }
}
