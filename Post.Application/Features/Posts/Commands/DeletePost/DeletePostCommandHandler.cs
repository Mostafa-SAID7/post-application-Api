using MediatR;
using Post.Application.Common.Interfaces;

namespace Post.Application.Features.Posts.Commands.DeletePost
{
    public class DeletePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeletePostCommand, bool>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.Id);

            if (post == null)
                return false;

            await _postRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
