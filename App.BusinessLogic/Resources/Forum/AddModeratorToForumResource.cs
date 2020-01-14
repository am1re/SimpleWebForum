using App.BusinessLogic.Resources.Identity.User;

namespace App.BusinessLogic.Resources.Forum
{
    public class AddModeratorToForumResource
    {
        public UserResource User { get; set; }
        public ForumResource Forum { get; set; }
    }
}
