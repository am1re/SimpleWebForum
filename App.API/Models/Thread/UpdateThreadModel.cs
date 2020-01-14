namespace App.API.Models.Thread
{
    public class UpdateThreadModel
    {
        public string Subject { get; set; }
        public int ParentForumId { get; set; }
    }
}
