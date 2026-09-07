namespace Post.Application.Features.Tags.Responses
{
    public class GetTagsResponse
    {
        public List<TagItemResponse> Items { get; set; } = [];
    }

    public class TagItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
