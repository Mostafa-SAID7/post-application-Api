using AutoMapper;
using Post.Application.Features.Posts.Commands.CreatePost;
using Post.Application.Features.Posts.Commands.UpdatePost;
using Post.Application.Features.Posts.Responses;
using Post.Domain.Entities;

namespace Post.Application.Features.Posts.Mapping
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            // CreatePost mappings
            CreateMap<CreatePostCommand, Domain.Entities.Post>();
            CreateMap<Domain.Entities.Post, CreatePostResponse>();

            // UpdatePost mappings
            CreateMap<UpdatePostCommand, Domain.Entities.Post>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<Domain.Entities.Post, UpdatePostResponse>();

            // GetPost mappings
            CreateMap<Domain.Entities.Post, GetPostResponse>();

            // GetPosts mappings
            CreateMap<Domain.Entities.Post, PostItemResponse>();

            // Search mappings
            CreateMap<Domain.Entities.Post, PostSearchItemResponse>();

            // PostDetailResponse as base for all detail responses
            CreateMap<Domain.Entities.Post, PostDetailResponse>();
        }
    }
}

