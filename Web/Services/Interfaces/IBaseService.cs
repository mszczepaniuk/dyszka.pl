using ApplicationCore.Models;
using ApplicationCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services.Interfaces
{
    public interface IBaseService<T, TRepo>
    {
        IQueryable<T> GetAll();
        PagedResult<T> GetPaged(int page);
        T GetById(Guid id);
        Task AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        Task<T> UpdateAsync(Guid id, T item);
        Task<bool> RemoveAsync(Guid id);
    }

    public interface IBaseService<T> : IBaseService<T, IBaseRepository<T>>
        where T : BaseEntity
    {

    }
}