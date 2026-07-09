using AutoMapper;
using Post.Application.Features.Categories.Responses;

namespace Post.Application.Features.Categories.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Domain.Entities.Category, CategoryDetailResponse>();
            CreateMap<Domain.Entities.Category, CreateCategoryResponse>();
            CreateMap<Domain.Entities.Category, UpdateCategoryResponse>();
            CreateMap<Domain.Entities.Category, GetCategoryResponse>();
            CreateMap<Domain.Entities.Category, CategoryItemResponse>();
        }
    }
}
