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

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("user-orders/{page}")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public IActionResult GetCreatedByCurrentUser(int page)
        {
            return Ok(orderService.GetCreatedByCurrentUser(page));
        }

        [HttpGet("user-offers/{page}/{done}")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public IActionResult GetCreatedByCurrentUser(int page, bool done)
        {
            return Ok(orderService.GetOrdersForCurrentUserOffers(page, done));
        }
    }
}