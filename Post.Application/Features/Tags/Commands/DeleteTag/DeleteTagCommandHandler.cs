using MediatR;
using Post.Application.Common.Interfaces;

namespace Post.Application.Features.Tags.Commands.DeleteTag
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, bool>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetByIdAsync(request.Id);
            if (tag == null)
                return false;

            await _tagRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
