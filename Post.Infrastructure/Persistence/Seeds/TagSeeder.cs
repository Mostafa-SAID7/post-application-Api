using Post.Domain.Entities;
using Post.Domain.ValueObjects;

namespace Post.Infrastructure.Persistence.Seeds
{
    /// <summary>
    /// Seeds tag data
    /// </summary>
    public class TagSeeder
    {
        public static List<Tag> GenerateTags()
        {
            return
            [
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "csharp",
                    Slug = Slug.GenerateSlug("csharp"),
                    Count = 5,
                    CreatedAt = DateTime.UtcNow
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "dotnet",
                    Slug = Slug.GenerateSlug("dotnet"),
                    Count = 4,
                    CreatedAt = DateTime.UtcNow
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "efcore",
                    Slug = Slug.GenerateSlug("efcore"),
                    Count = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "docker",
                    Slug = Slug.GenerateSlug("docker"),
                    Count = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "kubernetes",
                    Slug = Slug.GenerateSlug("kubernetes"),
                    Count = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "restapi",
                    Slug = Slug.GenerateSlug("restapi"),
                    Count = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "cleancode",
                    Slug = Slug.GenerateSlug("cleancode"),
                    Count = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "testing",
                    Slug = Slug.GenerateSlug("testing"),
                    Count = 2,
                    CreatedAt = DateTime.UtcNow
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "solidprinciples",
                    Slug = Slug.GenerateSlug("solidprinciples"),
                    Count = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "microservices",
                    Slug = Slug.GenerateSlug("microservices"),
                    Count = 2,
                    CreatedAt = DateTime.UtcNow
                }
            ];
        }
    }
}
