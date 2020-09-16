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

        public OffersController(IOfferService offerService,
            IMapper mapper)
        {
            this.offerService = offerService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(mapper.Map<OfferVm>(offerService.GetById(id)));
        }

        [HttpGet("/page/{page}")]
        public IActionResult GetPagedAndFiltered(int page, IEnumerable<string> tags, string username)
        {
            return Ok(offerService.GetPagedAndFiltered(page, tags, username));
        }

        [HttpPost]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> AddOffer(OfferBm offer)
        {
            return Ok(await offerService.AddAsync(mapper.Map<Offer>(offer)));
        }
    }
}