using MediatR;
using Post.Application.Features.Tags.Responses;

namespace Post.Application.Features.Tags.Queries.GetTag
{
    public class GetTagQuery : IRequest<GetTagResponse>
    {
        public Guid Id { get; set; }
    }
}
