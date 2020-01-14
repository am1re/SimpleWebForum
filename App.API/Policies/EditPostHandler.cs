using App.BusinessLogic.Identity;
using App.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.API.Authorization
{
    public class EditPostHandler : AuthorizationHandler<EditPostRequirement, int>
    {
        private readonly IPostService _postService;
        private readonly IThreadService _threadService;
        private readonly ForumUserManager _userService;

        public EditPostHandler(IPostService postService, ForumUserManager userService, IThreadService threadService)
        {
            _postService = postService;
            _userService = userService;
            _threadService = threadService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EditPostRequirement requirement, int postId)
        {
            var p = await _postService.GetPostById(postId);
            var thread = await _threadService.GetThreadById(p.ThreadId);
            var forumId = thread.ParentForum.Id;
            var canModerate = ModeratorOfForumHelper.CanModerateForum(context.User, forumId);
            if (canModerate)
            {
                context.Succeed(requirement);
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.FindByIdAsync(userId);
            var posts = user.Posts.Where(p => p.PostId == postId).Count();
            if (posts > 0)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }

    public class EditPostRequirement : IAuthorizationRequirement { }
}
