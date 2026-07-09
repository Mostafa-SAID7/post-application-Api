using MediatR;
using Post.Application.Features.Tags.Responses;

namespace Post.Application.Features.Tags.Queries.GetTags
{
    public class GetTagsQuery : IRequest<GetTagsResponse>
    {
        public bool PopularOnly { get; set; } = false;
        public int Take { get; set; } = 10;
    }
}
