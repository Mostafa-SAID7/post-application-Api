namespace Post.Application.Features.Posts.Responses
{
    public class GetPostsResponse
    {
        public List<PostItemResponse> Items { get; set; } = new();
    }

    public class PostItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
