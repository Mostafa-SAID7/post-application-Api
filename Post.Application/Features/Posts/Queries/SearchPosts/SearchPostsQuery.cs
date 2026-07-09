using MediatR;
using Post.Application.Common.Models;
using Post.Application.Features.Posts.Responses;

namespace Post.Application.Features.Posts.Queries.SearchPosts
{
    public class SearchPostsQuery : IRequest<SearchPostsResponse>
    {
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public List<Guid>? TagIds { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public int? MinViewCount { get; set; }

        // Sorting
        public string? SortBy { get; set; } = "created";
        public SortDirection SortDirection { get; set; } = SortDirection.Descending;

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public PostFilterParams ToFilterParams() => new()
        {
            SearchTerm = SearchTerm,
            CategoryId = CategoryId,
            TagIds = TagIds,
            CreatedAfter = CreatedAfter,
            CreatedBefore = CreatedBefore,
            MinViewCount = MinViewCount
        };

        public SortParams ToSortParams() => new()
        {
            SortBy = SortBy,
            SortDirection = SortDirection
        };

        public PaginationParams ToPaginationParams() => new()
        {
            PageNumber = PageNumber,
            PageSize = PageSize
        };
    }
}
