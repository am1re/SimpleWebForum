using System;

namespace App.API.Models.Post
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ThreadId { get; set; }
        public string User { get; set; }
    }
}
