using System;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public interface IAuditLogService
    {
        public PagedResult<AuditLog> GetPaged(int page);
        public PagedResult<T> GetPagedAndMapTo<T>(int page);
        public Task AddAuditLogAsync(string message, Guid? affectedElementId);
    }
}