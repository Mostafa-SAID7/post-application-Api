using MediatR;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Posts.Responses;

namespace Post.Application.Features.Posts.Queries.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, GetPostsResponse>
    {
        private readonly IPostRepository _postRepository;

        public GetPostsQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<GetPostsResponse> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllAsync();

            var items = posts.Select(p => new PostItemResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content
            }).ToList();

            return new GetPostsResponse { Items = items };
        }
    }
}
