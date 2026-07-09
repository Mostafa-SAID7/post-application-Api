using AutoMapper;
using Post.Application.Features.Tags.Responses;

namespace Post.Application.Features.Tags.Mapping
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Domain.Entities.Tag, TagDetailResponse>();
            CreateMap<Domain.Entities.Tag, CreateTagResponse>();
            CreateMap<Domain.Entities.Tag, UpdateTagResponse>();
            CreateMap<Domain.Entities.Tag, GetTagResponse>();
            CreateMap<Domain.Entities.Tag, TagItemResponse>();
        }
    }
}
