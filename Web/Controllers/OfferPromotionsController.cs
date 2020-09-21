using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Web.Authorization;
using Web.Services.Interfaces;

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

        [HttpGet]
        public IActionResult GetTagsAvailableForPromotion([FromQuery]string[] tags)
        {
            if (!tags.Any() || tags == null)
            {
                return BadRequest();
            }
            return Ok(offerPromotionService.GetTagsAvailableForPromotion(tags));
        }

        [HttpPost("{id}/{tag}")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> PromoteOffer(Guid id, string tag)
        {
            return await offerPromotionService.PromoteOffer(id, tag) ? StatusCode((int)HttpStatusCode.OK) : StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}