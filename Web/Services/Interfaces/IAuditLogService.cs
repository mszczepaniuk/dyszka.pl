using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace Web.Services.Interfaces
{
    public interface IAuditLogService
    {
        public PagedResult<AuditLog> GetPaged(int page);
        public PagedResult<T> GetPagedAndMapTo<T>(int page);
        public Task AddAuditLogAsync(string message, Guid? affectedElementId);
    }
}