using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Web.Authorization;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/offer-promotions")]
    public class OfferPromotionsController : Controller
    {
        private readonly IOfferPromotionService offerPromotionService;

        public OfferPromotionsController(IOfferPromotionService offerPromotionService)
        {
            this.offerPromotionService = offerPromotionService;
        }

        [HttpGet("{id}")]
        public IActionResult GetTagsAvailableForPromotion(Guid id, [FromQuery]string[] tags)
        {
            if (!tags.Any() || tags == null)
            {
                return BadRequest();
            }
            return Ok(offerPromotionService.GetTagsAvailableForPromotion(id, tags));
        }

        [HttpPost("{id}/{tag}")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> PromoteOffer(Guid id, string tag)
        {
            return await offerPromotionService.PromoteOffer(id, tag) ? StatusCode((int)HttpStatusCode.OK) : StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}