using App.API.Errors;
using App.API.Models.User;
using App.BusinessLogic.Interfaces;
using App.BusinessLogic.Resources.Identity.User;
using App.BusinessLogic.Resources.Paging;
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
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admins")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultResource<UserModel>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });

            var users = await _userService.GetAllPaged(page, pageSize);
            var result = new PagedResultResource<UserModel>(users)
            {
                Data = _mapper.Map<IEnumerable<UserModel>>(users.Data)
            };
            return Ok(result);
        }

        // GET: api/users/bob
        [HttpGet("{username}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserModel>> Get(string username)
        {
            var user = await _userService.GetUserByUserName(username);
            var result = _mapper.Map<UserModel>(user);
            return Ok(result);
        }

        // POST: api/users
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserModel>> Post([FromBody] CreateUserResource userBody)
        {
            var user = await _userService.CreateUser(userBody);
            var result = _mapper.Map<UserModel>(user);

            return CreatedAtAction(nameof(Get), new { userName = result.UserName }, result);
        }

        // PUT: api/users/bob
        [HttpPut("{username}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(string username, [FromBody] UpdateUserResource user)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userService.GetUserById(currentUserId);
            if (currentUser.UserName != username) return Forbid();
            if (!User.IsInRole("Admins")) return Forbid();

            await _userService.UpdateUser(username, user);
            return NoContent();
        }

        // DELETE: api/users/bob
        [HttpDelete("{username}")]
        [Authorize(Roles = "Admins")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string username)
        {
            await _userService.DeleteUser(username);
            return NoContent();
        }
    }
}