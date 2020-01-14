using App.Data.Entities.Identity;
using System;

namespace App.Data.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        //public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ThreadId { get; set; }
        public virtual Thread Thread { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
