using MediatR;

namespace Post.Application.Features.Posts.Commands.DeletePost
{
    public class DeletePostCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
