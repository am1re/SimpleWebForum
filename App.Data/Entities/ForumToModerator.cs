using App.Data.Entities.Identity;

namespace App.Data.Entities
{
    public class ForumToModerator
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int ForumId { get; set; }
        public virtual Forum Forum { get; set; }
    }
}
