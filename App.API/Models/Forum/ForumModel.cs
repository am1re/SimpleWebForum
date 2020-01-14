using System;
using System.Collections.Generic;

namespace App.API.Models.Forum
{
    public class ForumModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public int LastPostId { get; set; }
        public int PostsCount { get; set; }
        public int ThreadsCount { get; set; }

        public ICollection<string> Moderators { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
