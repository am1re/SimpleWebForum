using App.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Data.Entities
{
    public class Thread
    {
        public Thread()
        {
            Posts = new Collection<Post>();
        }

        public int ThreadId { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedAt { get; set; }
        // some attrs: locked, sticky, active, viewcount

        public int ForumId { get; set; }
        public virtual Forum Forum { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
