using App.BusinessLogic.Resources.Forum;
using App.BusinessLogic.Resources.Identity.User;
using App.BusinessLogic.Resources.Post;
using System;

namespace App.BusinessLogic.Resources.Thread
{
    public class ThreadResource
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime StartedAt { get; set; }

        // views?
        public int PostsCount { get; set; }
        public PostResource LastPost { get; set; }

        public ForumResource ParentForum { get; set; }
        public UserResource StartedBy { get; set; }
    }
}
