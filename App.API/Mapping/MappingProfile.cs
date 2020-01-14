using App.API.Models.Forum;
using App.API.Models.Post;
using App.API.Models.Thread;
using App.API.Models.User;
using App.BusinessLogic.Resources.Forum;
using App.BusinessLogic.Resources.Identity.User;
using App.BusinessLogic.Resources.Post;
using App.BusinessLogic.Resources.Thread;
using AutoMapper;
using System.Linq;

namespace App.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ForumResource, ForumModel>()
                .ForMember(r => r.Moderators, m => m.MapFrom(m => m.Moderators.Select(u => u.UserName)));

            CreateMap<ThreadResource, ThreadModel>()
                .ForMember(r => r.StartedBy, m => m.MapFrom(m => m.StartedBy.UserName));

            CreateMap<UserResource, UserModel>()
                .ForMember(r => r.Roles, m => m.MapFrom(m => m.Roles.Select(r => r.Name)));

            CreateMap<PostResource, PostModel>()
                .ForMember(r => r.User, m => m.MapFrom(m => m.User.UserName));

            CreateMap<CreatePostResource, CreatePostModel>()
                .ForMember(r => r.User, m => m.MapFrom(m => m.User.UserName))
                .ReverseMap();

            CreateMap<CreateThreadResource, CreateThreadModel>()
                .ForMember(r => r.StartedBy, m => m.MapFrom(m => m.StartedBy.UserName))
                .ReverseMap();

            CreateMap<UpdateThreadResource, UpdateThreadModel>()
                .ReverseMap();
        }
    }
}
