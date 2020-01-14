using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace App.API.Authorization
{
    public class ModeratorOfForumHandler : AuthorizationHandler<ModeratorOfForumRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ModeratorOfForumRequirement requirement, int forumId)
        {
            if (ModeratorOfForumHelper.CanModerateForum(context.User, forumId)) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class ModeratorOfForumRequirement : IAuthorizationRequirement { }
}
