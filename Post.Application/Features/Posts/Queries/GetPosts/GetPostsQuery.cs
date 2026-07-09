using MediatR;
using Post.Application.Features.Posts.Responses;

namespace Post.Application.Features.Posts.Queries.GetPosts
{
    public class GetPostsQuery : IRequest<GetPostsResponse>
    {
    }
}
