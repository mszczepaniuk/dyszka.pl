using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IBaseRepository<AuditLog> repository;
        private readonly int resultsPerPage = 20;

        public AuditLogService(IBaseRepository<AuditLog> repository)
        {
            this.repository = repository;
        }

        public PagedResult<AuditLog> GetPaged(int page)
        {
            return new PagedResult<AuditLog>
            {
                Items = repository.GetAll().Skip((page - 1) * resultsPerPage).Take(resultsPerPage).ToList(),
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