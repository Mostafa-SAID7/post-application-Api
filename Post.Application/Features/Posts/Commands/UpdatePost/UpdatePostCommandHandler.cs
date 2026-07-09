using MediatR;
using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Posts.Responses;

namespace Post.Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, UpdatePostResponse>
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePostCommandHandler(
            IPostRepository postRepository,
            ITagRepository tagRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdatePostResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdWithTagsAsync(request.Id);

            if (post == null)
                throw new EntityNotFoundException("Post", request.Id);

            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
                if (category == null)
                    throw new EntityNotFoundException("Category", request.CategoryId.Value);
            }

            post.Title = request.Title;
            post.Content = request.Content;
            post.Summary = request.Summary;
            post.CategoryId = request.CategoryId;

            post.Tags.Clear();
            if (request.TagIds?.Any() == true)
            {
                foreach (var tagId in request.TagIds)
                {
                    var tag = await _tagRepository.GetByIdAsync(tagId);
                    if (tag != null)
                        post.Tags.Add(tag);
                }
            }

            await _postRepository.UpdateAsync(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdatePostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Summary = post.Summary,
                Slug = post.Slug,
                CategoryId = post.CategoryId,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt ?? DateTime.UtcNow
            };
        }
    }
}
