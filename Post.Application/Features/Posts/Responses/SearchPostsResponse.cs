using Post.Application.Common.Models;

namespace Post.Application.Features.Posts.Responses
{
    public class SearchPostsResponse
    {
        public List<PostSearchItemResponse> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    public class PostSearchItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string Slug { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
