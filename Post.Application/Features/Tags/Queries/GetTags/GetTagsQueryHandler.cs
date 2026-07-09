using MediatR;
using Post.Application.Common.Interfaces;
using Post.Application.Features.Tags.Responses;

namespace Post.Application.Features.Tags.Queries.GetTags
{
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, GetTagsResponse>
    {
        private readonly ITagRepository _tagRepository;

        public GetTagsQueryHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<GetTagsResponse> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.Tag> tags;

            if (request.PopularOnly)
                tags = await _tagRepository.GetPopularTagsAsync(request.Take);
            else
                tags = await _tagRepository.GetAllAsync();

            var items = tags.Select(t => new TagItemResponse
            {
                Id = t.Id,
                Name = t.Name,
                Slug = t.Slug,
                Description = t.Description,
                Count = t.Count,
                CreatedAt = t.CreatedAt
            }).ToList();

            return new GetTagsResponse { Items = items };
        }
    }
}
