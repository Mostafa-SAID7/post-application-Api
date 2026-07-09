using MediatR;
using Post.Application.Features.Posts.Responses;

namespace Post.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<CreatePostResponse>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public Guid? CategoryId { get; set; }
        public List<Guid>? TagIds { get; set; } = new();
    }
}
