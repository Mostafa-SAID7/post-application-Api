using MediatR;
using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Tags.Responses;

namespace Post.Application.Features.Tags.Commands.UpdateTag
{
    public class UpdateTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateTagCommand, UpdateTagResponse>
    {
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UpdateTagResponse> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetByIdAsync(request.Id) ?? throw new EntityNotFoundException("Tag", request.Id);
            tag.Name = request.Name;
            tag.Description = request.Description;

            await _tagRepository.UpdateAsync(tag);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateTagResponse
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug,
                Description = tag.Description,
                Count = tag.Count,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt
            };
        }
    }
}
