using Post.Domain.Entities;
using Post.Domain.ValueObjects;

namespace Post.Infrastructure.Persistence.Seeds
{
    /// <summary>
    /// Seeds post data
    /// </summary>
    public class PostSeeder
    {
        public static List<Domain.Entities.Post> GeneratePosts(List<Category> categories, List<Tag> tags)
        {
            return new List<Domain.Entities.Post>
            {
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Getting Started with Clean Architecture",
                    Content = "Learn the fundamentals of layered architecture and SOLID principles for scalable applications. Clean architecture helps separate concerns and make your codebase more maintainable and testable.",
                    Slug = Slug.GenerateSlug("Getting Started with Clean Architecture"),
                    Summary = "Introduction to clean architecture patterns",
                    CategoryId = categories[0].Id,
                    ViewCount = 150,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    Tags = new List<Tag> { tags[0], tags[1] }
                },
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Advanced C# Patterns",
                    Content = "Deep dive into LINQ, expression trees, and advanced language features for expert developers. Master techniques like LINQ query composition, reflection, and dynamic programming.",
                    Slug = Slug.GenerateSlug("Advanced C# Patterns"),
                    Summary = "Master advanced C# programming techniques",
                    CategoryId = categories[1].Id,
                    ViewCount = 200,
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    Tags = new List<Tag> { tags[0], tags[8] }
                },
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Entity Framework Best Practices",
                    Content = "How to optimize your database queries and avoid common pitfalls with EF Core. Learn about lazy loading, eager loading, and query performance tuning.",
                    Slug = Slug.GenerateSlug("Entity Framework Best Practices"),
                    Summary = "Optimize EF Core queries for better performance",
                    CategoryId = categories[3].Id,
                    ViewCount = 180,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    Tags = new List<Tag> { tags[2], tags[1] }
                },
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "REST API Design Principles",
                    Content = "Building scalable, well-designed RESTful APIs with proper error handling and versioning. Includes best practices for HTTP methods, status codes, and API documentation.",
                    Slug = Slug.GenerateSlug("REST API Design Principles"),
                    Summary = "Design RESTful APIs that scale",
                    CategoryId = categories[1].Id,
                    ViewCount = 220,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    Tags = new List<Tag> { tags[5], tags[1] }
                },
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Docker Fundamentals",
                    Content = "Containerizing your applications with Docker and best practices for production deployments. Learn about Dockerfiles, images, containers, and Docker Compose.",
                    Slug = Slug.GenerateSlug("Docker Fundamentals"),
                    Summary = "Getting started with Docker containers",
                    CategoryId = categories[2].Id,
                    ViewCount = 160,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    Tags = new List<Tag> { tags[3], tags[4] }
                },
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Unit Testing Strategies",
                    Content = "Comprehensive guide to testing with xUnit, Moq, and best practices for maintainable tests. Covers unit tests, integration tests, and test-driven development.",
                    Slug = Slug.GenerateSlug("Unit Testing Strategies"),
                    Summary = "Master unit testing in C#",
                    CategoryId = categories[4].Id,
                    ViewCount = 140,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    Tags = new List<Tag> { tags[0], tags[7] }
                },
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Kubernetes Deployment Guide",
                    Content = "Orchestrating containers in production with Kubernetes, services, and ingress configuration. Learn about Pods, Services, Deployments, and StatefulSets.",
                    Slug = Slug.GenerateSlug("Kubernetes Deployment Guide"),
                    Summary = "Deploy and manage applications with Kubernetes",
                    CategoryId = categories[2].Id,
                    ViewCount = 120,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    Tags = new List<Tag> { tags[4], tags[3] }
                },
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Clean Code Principles",
                    Content = "Writing readable, maintainable code following SOLID principles and design patterns. Includes naming conventions, function design, error handling, and refactoring.",
                    Slug = Slug.GenerateSlug("Clean Code Principles"),
                    Summary = "Apply clean code principles to your projects",
                    CategoryId = categories[0].Id,
                    ViewCount = 190,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Tags = new List<Tag> { tags[8], tags[7] }
                },
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Microservices Architecture",
                    Content = "Building distributed systems with microservices patterns. Learn about service decomposition, inter-service communication, and consistency patterns.",
                    Slug = Slug.GenerateSlug("Microservices Architecture"),
                    Summary = "Design and implement microservices",
                    CategoryId = categories[1].Id,
                    ViewCount = 175,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    Tags = new List<Tag> { tags[9], tags[1] }
                },
                new Domain.Entities.Post
                {
                    Id = Guid.NewGuid(),
                    Title = "Database Optimization Techniques",
                    Content = "Learn advanced database optimization techniques including indexing strategies, query optimization, and performance monitoring.",
                    Slug = Slug.GenerateSlug("Database Optimization Techniques"),
                    Summary = "Optimize database performance",
                    CategoryId = categories[3].Id,
                    ViewCount = 145,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    Tags = new List<Tag> { tags[2], tags[1] }
                }
            };
        }
    }
}
