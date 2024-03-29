﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApplicationCore.BindingModels;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Services;
using ApplicationCore.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Authorization;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public CommentsController(ICommentService commentService,
            IMapper mapper,
            IAuthorizationService authorizationService)
        {
            this.commentService = commentService;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public IActionResult GetPaged([FromQuery]int? page, [FromQuery]string profileUsername, [FromQuery]Guid? offerId)
        {
            return !page.HasValue ? 
                Ok(mapper.Map<List<CommentVm>>(commentService.GetAll())) :
                Ok(commentService.GetPagedAndFiltered(page.Value, profileUsername, offerId));
        }

        [HttpPost]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> AddComment(CommentBm comment)
        {
            try
            {
                await commentService.AddAsync(mapper.Map<Comment>(comment));
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

        [HttpPut("{id}/toPositive")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> SetToPositive(Guid id)
        {
            try
            {
                if (!(await authorizationService.AuthorizeAsync(HttpContext.User, commentService.GetByIdAsNoTracking(id),
                    AuthConstants.IsOwnerPolicy)).Succeeded)
                {
                    return StatusCode((int)HttpStatusCode.Forbidden);
                }
                await commentService.SetToPositive(id);
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException is ElementNotFoundException)
                {
                    return NotFound(e.InnerException.Message);
                }
                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthConstants.NotBannedPolicy)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (!(await authorizationService.AuthorizeAsync(HttpContext.User, AuthConstants.ModeratorOrAdminPolicy))
                    .Succeeded && !(await authorizationService.AuthorizeAsync(HttpContext.User,
                    commentService.GetByIdAsNoTracking(id), AuthConstants.IsOwnerPolicy)).Succeeded)
                {
                    return StatusCode((int)HttpStatusCode.Forbidden);
                }
                await commentService.RemoveAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException is ElementNotFoundException)
                {
                    return NotFound(e.InnerException.Message);
                }
                throw;
            }
        }
    }
}