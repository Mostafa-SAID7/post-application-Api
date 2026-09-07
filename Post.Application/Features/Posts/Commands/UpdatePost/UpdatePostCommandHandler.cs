using MediatR;
using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Posts.Responses;

namespace Post.Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandHandler(
        IPostRepository postRepository,
        ITagRepository tagRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdatePostCommand, UpdatePostResponse>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UpdatePostResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdWithTagsAsync(request.Id) ?? throw new EntityNotFoundException("Post", request.Id);
            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value) ?? throw new EntityNotFoundException("Category", request.CategoryId.Value);
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
