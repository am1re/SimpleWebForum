using App.BusinessLogic.Resources.Post;
using App.BusinessLogic.Resources.Thread;
using App.Data.Entities;
using AutoMapper;
using System.Linq;

namespace App.BusinessLogic.Mapping.ValueResolvers.ForumResolvers
{
    public class LastPostThreadResolver : IValueResolver<Thread, ThreadResource, PostResource>
    {
        public LastPostThreadResolver() { }

        public PostResource Resolve(Thread source, ThreadResource destination, PostResource destMember, ResolutionContext context)
        {
            var post = source.Posts.OrderByDescending(t => t.CreatedAt).First();

            destMember = context.Mapper.Map<Post, PostResource>(post);

            return destMember;
        }
    }
}
