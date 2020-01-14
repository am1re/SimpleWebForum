using App.BusinessLogic.Resources.Identity.User;
using System;

namespace App.BusinessLogic.Resources.Post
{
    public class PostResource
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ThreadId { get; set; }
        public UserResource User { get; set; }

        // upvotes ?
    }
}
