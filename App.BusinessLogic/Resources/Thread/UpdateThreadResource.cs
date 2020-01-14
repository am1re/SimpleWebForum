using App.BusinessLogic.Resources.Forum;

namespace App.BusinessLogic.Resources.Thread
{
    public class UpdateThreadResource
    {
        public string Subject { get; set; }
        public ForumResource ParentForum { get; set; }
    }
}
