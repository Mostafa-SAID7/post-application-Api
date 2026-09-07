using MediatR;
using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Tags.Responses;

namespace Post.Application.Features.Tags.Queries.GetTag
{
    public class GetTagQueryHandler(ITagRepository tagRepository) : IRequestHandler<GetTagQuery, GetTagResponse>
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<GetTagResponse> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            var tag = await _tagRepository.GetByIdAsync(request.Id) ?? throw new EntityNotFoundException("Tag", request.Id);
            return new GetTagResponse
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
