using MediatR;
using Post.Application.Features.Categories.Responses;

namespace Post.Application.Features.Categories.Queries.GetCategory
{
    public class GetCategoryQuery : IRequest<GetCategoryResponse>
    {
        public Guid Id { get; set; }
    }
}
