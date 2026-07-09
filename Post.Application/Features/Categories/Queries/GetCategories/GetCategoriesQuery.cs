using MediatR;
using Post.Application.Features.Categories.Responses;

namespace Post.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<GetCategoriesResponse>
    {
        public Guid? ParentCategoryId { get; set; }
        public bool RootOnly { get; set; } = false;
    }
}
