using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Authorization;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpGet("{page}")]
        [Authorize(AuthConstants.OnlyAdminPolicy)]
        public IActionResult GetPaged(int page)
        {
            return Ok(paymentService.GetPaged(page));
        }

        [HttpPut("{id}")]
        [Authorize(AuthConstants.OnlyAdminPolicy)]
        public async Task<IActionResult> MarkAsDone(Guid id)
        {
            await paymentService.MarkAsDone(id);
            return Ok();
        }
    }
}