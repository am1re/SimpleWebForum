using App.BusinessLogic.Resources.Forum;
using App.BusinessLogic.Resources.Post;
using App.Data.Entities;
using AutoMapper;
using System.Linq;

namespace App.BusinessLogic.Mapping.ValueResolvers.ForumResolvers
{
    public class LastPostForumResolver : IValueResolver<Forum, ForumResource, PostResource>
    {
        public LastPostForumResolver() { }

        public PostResource Resolve(Forum source, ForumResource destination, PostResource destMember, ResolutionContext context)
        {
            var lastPost = new Post();
            foreach (var thread in source.Threads)
            {
                lastPost = thread.Posts.OrderByDescending(t => t.CreatedAt).First();
            }
            destMember = context.Mapper.Map<Post, PostResource>(lastPost);

            return destMember;
        }
    }
}
