using App.BusinessLogic.Resources.Identity.User;

namespace App.BusinessLogic.Resources.Post
{
    public class CreatePostResource
    {
        public string Content { get; set; }
        public int ThreadId { get; set; }
        public UserResource User { get; set; }
    }
}
