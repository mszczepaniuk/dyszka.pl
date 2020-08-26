using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Model;
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
                await userManager.AddClaimAsync(user, new Claim("CustomType", "CustomValue"));
                return Ok();
            }
            return BadRequest(result.Errors);
        }
    }
}