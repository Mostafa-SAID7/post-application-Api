using MediatR;
using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Posts.Responses;
using Post.Domain.ValueObjects;

namespace Post.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler(
        IPostRepository postRepository,
        ITagRepository tagRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreatePostCommand, CreatePostResponse>
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CreatePostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value) ?? throw new EntityNotFoundException("Category", request.CategoryId.Value);
            }

            var baseSlug = Slug.GenerateSlug(request.Title);
            var slug = baseSlug;
            var counter = 1;
            while (await _postRepository.ExistsAsync(slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            var post = new Domain.Entities.Post
            {
                Title = request.Title,
                Content = request.Content,
                Summary = request.Summary,
                Slug = slug,
                CategoryId = request.CategoryId
            };

            if (request.TagIds?.Any() == true)
            {
                foreach (var tagId in request.TagIds)
                {
                    var tag = await _tagRepository.GetByIdAsync(tagId);
                    if (tag != null)
                        post.Tags.Add(tag);
                }
            }

            await _postRepository.AddAsync(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreatePostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Summary = post.Summary,
                Slug = post.Slug,
                CategoryId = post.CategoryId,
                CreatedAt = post.CreatedAt
            };
        }
    }
}
