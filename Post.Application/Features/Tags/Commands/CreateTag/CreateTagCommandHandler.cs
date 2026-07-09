using MediatR;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Tags.Responses;
using Post.Domain.ValueObjects;

namespace Post.Application.Features.Tags.Commands.CreateTag
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, CreateTagResponse>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTagResponse> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            if (await _tagRepository.ExistsAsync(request.Name))
                throw new InvalidOperationException($"A tag with name '{request.Name}' already exists.");

            var baseSlug = Slug.GenerateSlug(request.Name);
            var slug = baseSlug;
            var counter = 1;
            while (await _tagRepository.GetBySlugAsync(slug) != null)
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            var tag = new Domain.Entities.Tag
            {
                Name = request.Name,
                Slug = slug,
                Description = request.Description,
                Count = 0
            };

            await _tagRepository.AddAsync(tag);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateTagResponse
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug,
                Description = tag.Description,
                Count = tag.Count,
                CreatedAt = tag.CreatedAt
            };
        }
    }
}
