namespace Post.Application.Features.Posts.Responses
{
    /// <summary>
    /// Unified response for post detail operations (Get, Create, Update)
    /// </summary>
    public class PostDetailResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string Slug { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
