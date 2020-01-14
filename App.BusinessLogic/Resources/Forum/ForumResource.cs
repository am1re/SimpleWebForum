using App.BusinessLogic.Resources.Identity.User;
using App.BusinessLogic.Resources.Post;
using System;
using System.Collections.Generic;

namespace App.BusinessLogic.Resources.Forum
{
    public class ForumResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ThreadsCount { get; set; }
        public int PostsCount { get; set; }
        public PostResource LastPost { get; set; }

        public ICollection<UserResource> Moderators { get; set; }

        // last activity (?), number of views (?)
    }
}
