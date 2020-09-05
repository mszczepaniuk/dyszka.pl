using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Authorization;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    //TODO: Authorization
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{username}")]
        public IActionResult GetByUserName(string username)
        {
            var user = userService.GetByUserName(username);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpGet("identity/{username}")]
        public async Task<IActionResult> GetIdentityData(string username)
        {
            var data = await userService.GetUserIdentityData(username);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest();
        }

        [HttpGet("admins/all")]
        public async Task<IActionResult> GetAllAdmins()
        {
            return Ok(await userService.GetAllInRole(AuthConstants.AdminRoleName));
        }

        [HttpGet("moderators/all")]
        public async Task<IActionResult> GetAllModerators()
        {
            return Ok(await userService.GetAllInRole(AuthConstants.ModeratorRoleName));
        }

        [HttpPut("add-admin/{username}")]
        public async Task<IActionResult> AddToAdmins(string username)
        {
            return await userService.AddToRole(username, AuthConstants.AdminRoleName)
                ? StatusCode((int) HttpStatusCode.OK)
                : StatusCode((int) HttpStatusCode.BadRequest);
        }

        [HttpPut("add-moderator/{username}")]
        public async Task<IActionResult> AddToModerators(string username)
        {
            return await userService.AddToRole(username, AuthConstants.ModeratorRoleName)
                ? StatusCode((int)HttpStatusCode.OK)
                : StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpPut("remove-admin/{username}")]
        public async Task<IActionResult> RemoveFromAdmins(string username)
        {
            return await userService.RemoveFromRole(username, AuthConstants.AdminRoleName)
                ? StatusCode((int)HttpStatusCode.OK)
                : StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpPut("remove-moderator/{username}")]
        public async Task<IActionResult> RemoveFromToModerators(string username)
        {
            return await userService.RemoveFromRole(username, AuthConstants.ModeratorRoleName)
                ? StatusCode((int)HttpStatusCode.OK)
                : StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpPut("ban/{username}")]
        public async Task<IActionResult> BanUser(string username)
        {
            return await userService.BanUser(username)
                ? StatusCode((int)HttpStatusCode.OK)
                : StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpPut("unban/{username}")]
        public async Task<IActionResult> UnbanUser(string username)
        {
            return await userService.UnbanUser(username)
                ? StatusCode((int)HttpStatusCode.OK)
                : StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> RemoveUser(string username)
        {
            var user = userService.GetByUserName(username);
            if (user == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            return await userService.RemoveAsync(user.Id)
                ? StatusCode((int)HttpStatusCode.OK)
                : StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}