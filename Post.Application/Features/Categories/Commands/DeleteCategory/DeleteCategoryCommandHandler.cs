using MediatR;
using Post.Application.Common.Interfaces;

namespace Post.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                return false;

            await _categoryRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
