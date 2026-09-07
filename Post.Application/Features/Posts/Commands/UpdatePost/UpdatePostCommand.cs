using MediatR;
using Post.Application.Features.Posts.Responses;

namespace Post.Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostCommand : IRequest<UpdatePostResponse>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public Guid? CategoryId { get; set; }
        public List<Guid>? TagIds { get; set; } = [];
    }
}
