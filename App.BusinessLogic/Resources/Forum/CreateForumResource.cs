namespace App.BusinessLogic.Resources.Forum
{
    public class CreateForumResource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
