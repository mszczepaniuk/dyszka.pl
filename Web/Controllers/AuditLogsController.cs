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
    [Route("api/audit-logs")]
    [Authorize(AuthConstants.OnlyAdminPolicy)]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService auditLogService;
        private readonly IMapper mapper;

        public AuditLogsController(IAuditLogService auditLogService,
            IMapper mapper)
        {
            this.auditLogService = auditLogService;
            this.mapper = mapper;
        }

        [HttpGet("{page}")]
        public IActionResult GetPaged(int page)
        {
            if (page < 1)
            {
                return BadRequest("Page number must be bigger than 1.");
            }
            return Ok(auditLogService.GetPagedAndMapTo<AuditLogVm>(page));
        }
    }
}