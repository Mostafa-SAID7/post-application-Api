using Post.Domain.Entities;
using Post.Domain.ValueObjects;

namespace Post.Infrastructure.Persistence.Seeds
{
    /// <summary>
    /// Seeds category data
    /// </summary>
    public class CategorySeeder
    {
        public static List<Category> GenerateCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Architecture",
                    Slug = Slug.GenerateSlug("Architecture"),
                    Description = "Software architecture patterns and best practices",
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Backend Development",
                    Slug = Slug.GenerateSlug("Backend Development"),
                    Description = "Backend technologies and frameworks",
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "DevOps",
                    Slug = Slug.GenerateSlug("DevOps"),
                    Description = "DevOps practices and tools",
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Database",
                    Slug = Slug.GenerateSlug("Database"),
                    Description = "Database design and optimization",
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Testing",
                    Slug = Slug.GenerateSlug("Testing"),
                    Description = "Unit testing, integration testing, and QA",
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}
