using AutoMapper;
using MediatR;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Posts.Responses;

namespace Post.Application.Features.Posts.Queries.SearchPosts
{
    public class SearchPostsHandler(IPostRepository postRepository, IMapper mapper) : IRequestHandler<SearchPostsQuery, SearchPostsResponse>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<SearchPostsResponse> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
        {
            var filters = request.ToFilterParams();
            var sort = request.ToSortParams();
            var pagination = request.ToPaginationParams();

            var pagedResult = await _postRepository.SearchAsync(filters, sort, pagination);

            var response = new SearchPostsResponse
            {
                Items = _mapper.Map<List<PostSearchItemResponse>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalPages = pagedResult.TotalPages,
                HasPreviousPage = pagedResult.HasPreviousPage,
                HasNextPage = pagedResult.HasNextPage
            };

            return response;
        }
    }
}
