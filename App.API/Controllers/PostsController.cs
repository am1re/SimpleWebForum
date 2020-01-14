using App.API.Errors;
using App.API.Models.Post;
using App.BusinessLogic.Interfaces;
using App.BusinessLogic.Resources.Paging;
using App.BusinessLogic.Resources.Post;
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
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IThreadService _threadService;

        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public PostsController(IMapper mapper,
                               IPostService postService,
                               IThreadService threadService,
                               IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _postService = postService;
            _threadService = threadService;
            _authorizationService = authorizationService;
        }

        // GET: api/posts
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultResource<PostModel>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });
            
            var posts = await _postService.GetAllPaged(page, pageSize);
            var result = new PagedResultResource<PostModel>(posts)
            {
                Data = _mapper.Map<IEnumerable<PostModel>>(posts.Data)
            };
            return Ok(result);
        }

        // GET: api/posts/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostModel>> Get(int id)
        {
            var post = await _postService.GetPostById(id);
            var result = _mapper.Map<PostModel>(post);
            return Ok(result);
        }

        // POST: api/posts
        [HttpPost]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostModel>> Post([FromBody] CreatePostModel postBody)
        {
            var postBodyResource = _mapper.Map<CreatePostResource>(postBody);
            var post = await _postService.CreatePost(postBodyResource);
            var result = _mapper.Map<PostModel>(post);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/posts/5
        [HttpPut("{id}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdatePostResource post)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, id, "EditPost")).Succeeded)
            {
                return Forbid();
            }

            await _postService.UpdatePost(id, post);
            return NoContent();
        }

        // DELETE: api/posts/5
        [HttpDelete("{id}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var p = await _postService.GetPostById(id);
            var thread = await _threadService.GetThreadById(p.ThreadId);
            var forumId = thread.ParentForum.Id;
            if (!(await _authorizationService.AuthorizeAsync(User, forumId, "ModeratorOfForum")).Succeeded)
            {
                return Forbid();
            }

            await _postService.DeletePost(id);
            return NoContent();
        }
    }
}