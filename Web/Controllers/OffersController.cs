using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApplicationCore.BindingModels;
using ApplicationCore.Exceptions;
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
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService offerService;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;
        private readonly IOrderService orderService;

        public OffersController(IOfferService offerService,
            IMapper mapper,
            IAuthorizationService authorizationService,
            IOrderService orderService)
        {
            this.offerService = offerService;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
            this.orderService = orderService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(mapper.Map<OfferVm>(offerService.GetById(id)));
        }

        [HttpGet]
        public IActionResult GetPagedAndFiltered([FromQuery]int? page, [FromQuery]string[] tags, [FromQuery]string username)
        {
            if (!page.HasValue)
            {
                return Ok(mapper.Map<List<OfferVm>>(offerService.GetAll()));
            }
            return Ok(offerService.GetPagedAndFiltered(page.Value, tags, username));
        }

        [HttpPost]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> AddOffer(OfferBm offer)
        {
            return Ok(await offerService.AddAsync(mapper.Map<Offer>(offer)));
        }

        [HttpPut("{id}/hide")]
        [Authorize]
        public async Task<IActionResult> HideOffer(Guid id)
        {
            if (!(await authorizationService.AuthorizeAsync(HttpContext.User, offerService.GetAll()
                    .Where(offer => offer.Id == id).AsNoTracking().Include(offer => offer.CreatedBy).FirstOrDefault(),
                AuthConstants.IsOwnerPolicy)).Succeeded)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
            try
            {
                await offerService.HideOffer(id);
                return Ok();
            }
            catch (ElementNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}/show")]
        [Authorize]
        public async Task<IActionResult> ShowOffer(Guid id)
        {
            if (!(await authorizationService.AuthorizeAsync(HttpContext.User, offerService.GetAll()
                    .Where(offer => offer.Id == id).AsNoTracking().Include(offer => offer.CreatedBy).FirstOrDefault(),
                AuthConstants.IsOwnerPolicy)).Succeeded)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
            try
            {
                await offerService.ShowOffer(id);
                return Ok();
            }
            catch (ElementNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{id}/order")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> CreateOrder(Guid offerId)
        {
            await orderService.CreateOrder(offerId);
            return Ok();
        }
    }
}