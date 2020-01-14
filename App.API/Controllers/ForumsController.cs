using App.API.Errors;
using App.API.Models.Forum;
using App.API.Models.Thread;
using App.API.Models.User;
using App.BusinessLogic.Interfaces;
using App.BusinessLogic.Resources.Forum;
using App.BusinessLogic.Resources.Paging;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumsController : ControllerBase
    {
        private readonly IForumService _forumService;
        private readonly IThreadService _threadService;
        private readonly IUserService _userService;

        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public ForumsController(IMapper mapper,
                                IForumService forumService,
                                IThreadService threadService,
                                IUserService userService,
                                IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _forumService = forumService;
            _threadService = threadService;
            _userService = userService;
            _authorizationService = authorizationService;
        }

        // GET: api/forums
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultResource<ForumModel>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });
            
            var forums = await _forumService.GetAllPaged(page, pageSize);
            var result = new PagedResultResource<ForumModel>(forums)
            {
                Data = _mapper.Map<IEnumerable<ForumModel>>(forums.Data)
            };
            return Ok(result);
        }

        // GET: api/forums/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ForumModel>> Get(int id)
        {
            var forum = await _forumService.GetForumById(id);
            var result = _mapper.Map<ForumModel>(forum);
            return Ok(result);
        }

        // GET: api/forums/5/threads
        [HttpGet("{id}/Threads")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResultResource<ThreadModel>>> GetThreads(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });

            var threads = await _threadService.GetThreadsByForumIdPaged(id, page, pageSize);
            var result = new PagedResultResource<ThreadModel>(threads)
            {
                Data = _mapper.Map<IEnumerable<ThreadModel>>(threads.Data)
            };
            return Ok(result);
        }

        // GET: api/forums/5/threads-count
        [HttpGet("{id}/Threads-Count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetThreadsCount(int id)
        {
            var result = await _threadService.GetThreadsCountByForumId(id);
            return Ok(result);
        }

        // GET: api/forums/5/moderators
        [HttpGet("{id}/Moderators")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResultResource<UserModel>>> GetModerators(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });

            var users = await _userService.GetUsersAsForumModeratorsPaged(id, page, pageSize);
            var result = new PagedResultResource<UserModel>(users)
            {
                Data = _mapper.Map<IEnumerable<UserModel>>(users.Data)
            };
            return Ok(result);
        }

        // POST: api/forums
        [HttpPost]
        [Authorize(Roles = "GlobalModerators, Admins")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ForumModel>> Post([FromBody] CreateForumResource forumBody)
        {
            var forum = await _forumService.CreateForum(forumBody);
            var result = _mapper.Map<ForumModel>(forum);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/forums/5
        [HttpPut("{id}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateForumResource forum)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, id, "ModeratorOfForum")).Succeeded)
            {
                return Forbid();
            }

            await _forumService.UpdateForum(id, forum);
            return NoContent();
        }

        // DELETE: api/forums/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "GlobalModerators, Admins")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            await _forumService.DeleteForum(id);
            return NoContent();
        }
    }
}