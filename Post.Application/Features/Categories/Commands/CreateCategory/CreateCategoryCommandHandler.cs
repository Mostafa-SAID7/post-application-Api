using MediatR;
using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Categories.Responses;
using Post.Domain.ValueObjects;

namespace Post.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _categoryRepository.ExistsAsync(request.Name))
                throw new InvalidOperationException($"A category with name '{request.Name}' already exists.");

            if (request.ParentCategoryId.HasValue)
            {
                var parent = await _categoryRepository.GetByIdAsync(request.ParentCategoryId.Value) ?? throw new EntityNotFoundException("Category", request.ParentCategoryId.Value);
            }

            var baseSlug = Slug.GenerateSlug(request.Name);
            var slug = baseSlug;
            var counter = 1;
            while (await _categoryRepository.GetBySlugAsync(slug) != null)
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            var category = new Domain.Entities.Category
            {
                Name = request.Name,
                Slug = slug,
                Description = request.Description,
                ParentCategoryId = request.ParentCategoryId
            };

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                CreatedAt = category.CreatedAt
            };
        }
    }
}
