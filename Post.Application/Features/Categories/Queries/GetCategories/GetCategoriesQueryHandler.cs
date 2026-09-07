using MediatR;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Categories.Responses;

namespace Post.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoriesQuery, GetCategoriesResponse>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<GetCategoriesResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.Category> categories;

            if (request.RootOnly)
                categories = await _categoryRepository.GetRootCategoriesAsync();
            else if (request.ParentCategoryId.HasValue)
                categories = await _categoryRepository.GetSubCategoriesAsync(request.ParentCategoryId.Value);
            else
                categories = await _categoryRepository.GetAllAsync();

            var items = categories.Select(c => new CategoryItemResponse
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ParentCategoryId = c.ParentCategoryId,
                CreatedAt = c.CreatedAt
            }).ToList();

            return new GetCategoriesResponse { Items = items };
        }
    }
}
