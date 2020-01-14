using System.Linq;
using System.Security.Claims;

namespace App.API.Authorization
{
    public static class ModeratorOfForumHelper
    {
        public static bool CanModerateForum(ClaimsPrincipal User, int forumId)
        {
            var claims = User.Claims.Where(c => c.Type == "ModeratorOfForum" && c.Value == forumId.ToString()).Count();
            return (User.IsInRole("Admins") || User.IsInRole("GlobalModerators") || claims > 0);
        }
    }
}
