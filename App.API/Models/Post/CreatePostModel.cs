namespace App.API.Models.Post
{
    public class CreatePostModel
    {
        public string Content { get; set; }
        public int ThreadId { get; set; }
        public string User { get; set; }
    }
}
