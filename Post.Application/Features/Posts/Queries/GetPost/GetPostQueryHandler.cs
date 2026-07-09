using MediatR;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Posts.Responses;
using Post.Application.Common.Exceptions;

namespace Post.Application.Features.Posts.Queries.GetPost
{
    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, GetPostResponse>
    {
        private readonly IPostRepository _repository;

        public GetPostQueryHandler(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetPostResponse> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _repository.GetByIdAsync(request.Id);

            if (post == null)
                throw new EntityNotFoundException("Post", request.Id);

            return new GetPostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };
        }
    }
}
