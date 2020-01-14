namespace App.API.Models.Thread
{
    public class CreateThreadModel
    {
        public string Subject { get; set; }
        public int ParentForumId { get; set; }
        public string StartedBy { get; set; }
    }
}
