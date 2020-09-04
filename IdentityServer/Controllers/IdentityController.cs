using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Data;
using IdentityServer.Model;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<CustomIdentityUser> userManager;

        public IdentityController(UserManager<CustomIdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var user = new CustomIdentityUser { UserName = registerViewModel.Username };
            var result = await userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("claim/{username}/{role}")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> AddRoleToUser(string username, string role)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null || user.IsBanned)
            {
                return BadRequest("No user with given username was found or user is banned.");
            }
            try
            {
                await userManager.AddToRoleAsync(user, role);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("claim/{username}/{role}")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> RemoveRoleFromUser(string username, string role)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("No user with given username was found.");
            }
            try
            {
                await userManager.RemoveFromRoleAsync(user, role);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("role/{rolename}")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> GetAllUsersInRole(string rolename)
        {
            return Ok((await userManager.GetUsersInRoleAsync(rolename)).Select(u => u.UserName));
        }

        [HttpPost("ban/{username}")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public async Task<IActionResult> BanUser(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("No user with given username was found.");
            }
            user.IsBanned = true;
            await userManager.UpdateAsync(user);
            return Ok();
        }
    }
}