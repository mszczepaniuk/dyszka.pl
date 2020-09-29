using System;
using System.Net;
using System.Threading.Tasks;
using ApplicationCore.BindingModels;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Authorization;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public MessagesController(IMessageService messageService,
            IMapper mapper,
            IAuthorizationService authorizationService)
        {
            this.messageService = messageService;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

        [HttpGet("{page}/{user1}/{user2}")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> GetPagedAndFiltered(int page, string user1, string user2)
        {
            if (user1 == user2)
            {
                return BadRequest();
            }

            if (!(await authorizationService.AuthorizeAsync(HttpContext.User, user1, AuthConstants.GetMessagesPolicy))
                .Succeeded &&
                !(await authorizationService.AuthorizeAsync(HttpContext.User, user2, AuthConstants.GetMessagesPolicy))
                .Succeeded)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
            return Ok(messageService.GetPagedAndFiltered(page, user1, user2));
        }

        [HttpGet("{page}/{user}")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> GetReceivedMessages(int page, string user)
        {
            if (!(await authorizationService.AuthorizeAsync(HttpContext.User, user, AuthConstants.GetMessagesPolicy))
                    .Succeeded)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
            return Ok(messageService.GetLastReceivedMessages(page, user));
        }

        [HttpPost]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> Add(MessageBm message)
        {
            try
            {
                await messageService.AddAsync(mapper.Map<Message>(message));
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException is ElementNotFoundException)
                {
                    return BadRequest(e.InnerException.Message);
                }
                throw;
            }
        }
    }
}