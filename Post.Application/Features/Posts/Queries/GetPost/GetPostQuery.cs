using MediatR;
using Post.Application.Features.Posts.Responses;

namespace Post.Application.Features.Posts.Queries.GetPost
{
    public class GetPostQuery : IRequest<GetPostResponse>
    {
        public Guid Id { get; set; }
    }
}
