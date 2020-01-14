using App.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.API.Authorization
{
    public class EditThreadHandler : AuthorizationHandler<EditThreadRequirement, int>
    {
        private readonly IThreadService _threadService;

        public EditThreadHandler(IThreadService threadService)
        {
            _threadService = threadService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EditThreadRequirement requirement, int threadId)
        {
            var thread = await _threadService.GetThreadById(threadId);
            var forumId = thread.ParentForum.Id;
            var canModerate = ModeratorOfForumHelper.CanModerateForum(context.User, forumId);
            if (canModerate)
            {
                context.Succeed(requirement);
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isOwner = thread.StartedBy.Id == userId;
            if (isOwner)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }

    public class EditThreadRequirement : IAuthorizationRequirement { }
}
