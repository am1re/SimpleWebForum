using App.BusinessLogic.Resources.Forum;
using App.Data.Entities;
using AutoMapper;

namespace App.BusinessLogic.Mapping.ValueResolvers.ForumResolvers
{
    public class PostsCountForumResolver : IValueResolver<Forum, ForumResource, int>
    {
        public PostsCountForumResolver() { }

        public int Resolve(Forum source, ForumResource destination, int destMember, ResolutionContext context)
        {
            int count = 0;
            foreach (var thread in source.Threads)
            {
                count += thread.Posts.Count;
            }

            return count;
        }
    }
}
