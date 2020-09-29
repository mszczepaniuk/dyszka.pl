using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Web.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IBaseRepository<AuditLog> repository;
        private readonly IMapper mapper;
        private readonly int resultsPerPage = 20;

        public AuditLogService(IBaseRepository<AuditLog> repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public PagedResult<AuditLog> GetPaged(int page)
        {
            return new PagedResult<AuditLog>
            {
                Items = repository.GetAll().OrderByDescending(auditLog => auditLog.CreatedDate).Skip((page - 1) * resultsPerPage).Take(resultsPerPage).Include(a => a.CreatedBy).ToList(),
                CurrentPage = page,
                ResultsPerPage = resultsPerPage,
                PagesCount = repository.GetAll().Any() ?
                    (repository.GetAll().Count() - 1) / resultsPerPage + 1 :
                    0
            };
        }

        public virtual PagedResult<TVm> GetPagedAndMapTo<TVm>(int page)
        {
            return new PagedResult<TVm>
            {
                Items = mapper.Map<List<TVm>>(repository.GetAll().OrderByDescending(auditLog => auditLog.CreatedDate).Skip((page - 1) * resultsPerPage).Take(resultsPerPage).Include(a => a.CreatedBy).ToList()),
                CurrentPage = page,
                ResultsPerPage = resultsPerPage,
                PagesCount = repository.GetAll().Any() ?
                    (repository.GetAll().Count() - 1) / resultsPerPage + 1 :
                    0
            };
        }

        public async Task AddAuditLogAsync(string message, Guid? affectedElementId) =>
            await repository.AddAsync(new AuditLog {Message = message, AffectedEntityId = affectedElementId});
    }
}