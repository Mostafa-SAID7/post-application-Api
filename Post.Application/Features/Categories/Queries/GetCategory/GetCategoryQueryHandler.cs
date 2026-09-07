using MediatR;
using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Categories.Responses;

namespace Post.Application.Features.Categories.Queries.GetCategory
{
    public class GetCategoryQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryQuery, GetCategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<GetCategoryResponse> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id) ?? throw new EntityNotFoundException("Category", request.Id);
            return new GetCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }
    }
}
