using App.BusinessLogic.Mapping.ValueResolvers.ForumResolvers;
using App.BusinessLogic.Resources.Forum;
using App.BusinessLogic.Resources.Identity.Role;
using App.BusinessLogic.Resources.Identity.User;
using App.BusinessLogic.Resources.Post;
using App.BusinessLogic.Resources.Thread;
using App.Data.Entities;
using App.Data.Entities.Identity;
using AutoMapper;

namespace App.BusinessLogic.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Forum, ForumResource>()
                .ForMember(m => m.Id, f => f.MapFrom(f => f.ForumId))
                .ForMember(m => m.LastPost, f => f.MapFrom<LastPostForumResolver>())
                .ForMember(m => m.PostsCount, f => f.MapFrom<PostsCountForumResolver>())
                .ReverseMap();
            CreateMap<CreateForumResource, Forum>();

            CreateMap<Thread, ThreadResource>()
                .ForMember(m => m.Id, t => t.MapFrom(t => t.ThreadId))
                .ForMember(m => m.StartedAt, t => t.MapFrom(t => t.CreatedAt))
                .ForMember(m => m.StartedBy, t => t.MapFrom(t => t.User))
                .ForMember(m => m.ParentForum, t => t.MapFrom(t => t.Forum))
                .ForMember(m => m.LastPost, t => t.MapFrom<LastPostThreadResolver>())
                .ReverseMap();
            CreateMap<CreateThreadResource, Thread>();

            CreateMap<Post, PostResource>()
                .ForMember(m => m.Id, p => p.MapFrom(p => p.PostId))
                .ReverseMap();
            CreateMap<CreatePostResource, Post>();

            CreateMap<User, UserResource>()
                .ReverseMap();
            CreateMap<CreateUserResource, User>();

            CreateMap<Role, RoleResource>();
            CreateMap<UserToRole, RoleResource>()
                .IncludeMembers(u => u.Role);

            CreateMap<ForumToModerator, UserResource>()
                .IncludeMembers(u => u.User);
            CreateMap<AddModeratorToForumResource, ForumToModerator>();
        }
    }
}
