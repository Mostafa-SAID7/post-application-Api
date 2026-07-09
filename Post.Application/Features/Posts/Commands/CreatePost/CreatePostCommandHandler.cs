using MediatR;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Posts.Responses;
using Post.Domain.ValueObjects;

namespace Post.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CreatePostResponse>
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreatePostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            // Generate base slug
            var baseSlug = Slug.GenerateSlug(request.Title);
            var slug = baseSlug;

            // Ensure slug uniqueness by adding a suffix if needed
            var counter = 1;
            while (await _postRepository.ExistsAsync(slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            var post = new Domain.Entities.Post
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                Slug = slug
            };

            await _postRepository.AddAsync(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreatePostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };
        }
    }
}

