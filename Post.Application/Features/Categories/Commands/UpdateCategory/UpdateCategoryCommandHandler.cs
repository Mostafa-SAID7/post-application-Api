using MediatR;
using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Categories.Responses;

namespace Post.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id) ?? throw new EntityNotFoundException("Category", request.Id);
            if (request.ParentCategoryId.HasValue)
            {
                if (request.ParentCategoryId == request.Id)
                    throw new InvalidOperationException("A category cannot be its own parent.");

                var parent = await _categoryRepository.GetByIdAsync(request.ParentCategoryId.Value) ?? throw new EntityNotFoundException("Category", request.ParentCategoryId.Value);
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.ParentCategoryId = request.ParentCategoryId;

            await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateCategoryResponse
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
