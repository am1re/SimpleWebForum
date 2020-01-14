using System;

namespace App.API.Models.Thread
{
    public class ThreadModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime StartedAt { get; set; }

        public int PostsCount { get; set; }
        public int LastPostId { get; set; }

        public int ParentForumId { get; set; }
        public string StartedBy { get; set; }
    }
}
