using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Data.Entities
{
    public class Forum
    {
        public Forum()
        {
            Threads = new Collection<Thread>();
            Moderators = new Collection<ForumToModerator>();
        }

        public int ForumId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        // created by ?

        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<ForumToModerator> Moderators { get; set; }
    }
}
