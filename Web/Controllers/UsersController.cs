using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApplicationCore.BindingModels;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper mapper;

        public UsersController(IUserService userService,
            IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet("{username}")]
        public IActionResult GetByUserName(string username)
        {
            var user = userService.GetByUserName(username);
            if (user != null)
            {
                return Ok(mapper.Map<ApplicationUserVm>(user));
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

        [HttpPost("{id}")]
        public async Task<IActionResult> EditUser(Guid id, ApplicationUserBm userBm)
        {
            try
            {
                return Ok(await userService.UpdateAsync(id, mapper.Map<ApplicationUser>(userBm)));
            }
            catch (DbUpdateException e)
            {
                return BadRequest();
            }
        }

        [HttpGet("admins/all")]
        public async Task<IActionResult> GetAllAdmins()
        {
            return Ok(mapper.Map<List<ApplicationUserVm>>(await userService.GetAllInRole(AuthConstants.AdminRoleName)));
        }

        [HttpGet("moderators/all")]
        public async Task<IActionResult> GetAllModerators()
        {
            return Ok(mapper.Map<List<ApplicationUserVm>>(await userService.GetAllInRole(AuthConstants.ModeratorRoleName)));
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