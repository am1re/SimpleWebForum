using App.API.Errors;
using App.API.Models.Post;
using App.API.Models.Thread;
using App.BusinessLogic.Interfaces;
using App.BusinessLogic.Resources.Paging;
using App.BusinessLogic.Resources.Thread;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly IThreadService _threadService;
        private readonly IPostService _postService;

        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public ThreadsController(IMapper _mapper,
                                 IThreadService threadService,
                                 IPostService postService,
                                 IAuthorizationService authorizationService)
        {
            this._mapper = _mapper;
            _threadService = threadService;
            _postService = postService;
            _authorizationService = authorizationService;
        }

        // GET: api/threads
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultResource<ThreadModel>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });
            
            var threads = await _threadService.GetAllPaged(page, pageSize);
            var result = new PagedResultResource<ThreadModel>(threads)
            {
                Data = _mapper.Map<IEnumerable<ThreadModel>>(threads.Data)
            };
            return Ok(result);
        }

        // GET: api/threads/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ThreadModel>> Get(int id)
        {
            var thread = await _threadService.GetThreadById(id);
            var result = _mapper.Map<ThreadModel>(thread);
            return Ok(result);
        }

        // GET: api/threads/5/posts
        [HttpGet("{id}/Posts")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResultResource<PostModel>>> GetPosts(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });
            
            var posts = await _postService.GetPostsByThreadIdPaged(id, page, pageSize);
            var result = new PagedResultResource<PostModel>(posts)
            {
                Data = _mapper.Map<IEnumerable<PostModel>>(posts.Data)
            };
            return Ok(result);
        }

        // GET: api/threads/5/posts-count
        [HttpGet("{id}/Posts-count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetPostsCount(int id)
        {
            var result = await _postService.GetPostsCountByThreadId(id);
            return Ok(result);
        }

        // POST: api/threads
        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ThreadModel>> Post([FromBody] CreateThreadModel threadBody)
        {
            threadBody.StartedBy = (string.IsNullOrEmpty(threadBody.StartedBy)) ? User.FindFirstValue(ClaimTypes.Name) : threadBody.StartedBy;

            var threadBodyResource = _mapper.Map<CreateThreadResource>(threadBody);
            var thread = await _threadService.CreateThread(threadBodyResource);
            var result = _mapper.Map<ThreadModel>(thread);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/threads/5
        [Authorize]
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateThreadModel threadBody)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, id, "EditThread")).Succeeded)
            {
                return Forbid();
            }

            var threadResource = _mapper.Map<UpdateThreadResource>(threadBody);
            await _threadService.UpdateThread(id, threadResource);
            return NoContent();
        }

        // DELETE: api/threads/5
        [Authorize]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var t = await _threadService.GetThreadById(id);
            var forumId = t.ParentForum.Id;
            if (!(await _authorizationService.AuthorizeAsync(User, forumId, "ModeratorOfForum")).Succeeded)
            {
                return Forbid();
            }

            await _threadService.DeleteThread(id);
            return NoContent();
        }
    }
}