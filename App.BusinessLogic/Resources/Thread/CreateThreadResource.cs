using App.BusinessLogic.Resources.Forum;
using App.BusinessLogic.Resources.Identity.User;

namespace App.BusinessLogic.Resources.Thread
{
    public class CreateThreadResource
    {
        public string Subject { get; set; }
        public ForumResource ParentForum { get; set; }
        public UserResource StartedBy { get; set; }
    }
}
