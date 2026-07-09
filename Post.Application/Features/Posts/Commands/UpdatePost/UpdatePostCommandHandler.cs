using MediatR;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Posts.Responses;
using Post.Application.Common.Exceptions;

namespace Post.Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, UpdatePostResponse>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdatePostResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.Id);
            
            if (post == null)
                throw new EntityNotFoundException("Post", request.Id);

            post.Title = request.Title;
            post.Content = request.Content;

            await _postRepository.UpdateAsync(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdatePostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };
        }
    }
}
