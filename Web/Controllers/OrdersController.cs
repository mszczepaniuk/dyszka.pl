using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Authorization;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IAuthorizationService authorizationService;

        public OrdersController(IOrderService orderService,
            IAuthorizationService authorizationService)
        {
            this.orderService = orderService;
            this.authorizationService = authorizationService;
        }

        [HttpGet("user-orders/{page}")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public IActionResult GetCreatedByCurrentUser(int page)
        {
            return Ok(orderService.GetCreatedByCurrentUser(page));
        }

        [HttpGet("user-offers/{page}/{done}")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public IActionResult GetOrdersForCurrentUserOffers(int page, bool done)
        {
            return Ok(orderService.GetOrdersForCurrentUserOffers(page, done));
        }

        [HttpPost("{id}/done")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> MarkAsDone(Guid id)
        {
            if (!(await authorizationService.AuthorizeAsync(HttpContext.User,
                orderService.GetByIdAsNoTracking(id), AuthConstants.IsOwnerPolicy)).Succeeded)
            {
                return StatusCode((int) HttpStatusCode.Forbidden);
            }
            await orderService.MarkAsDone(id);
            return Ok();
        }
    }
}