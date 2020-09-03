using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services.Interfaces
{
    public interface IBaseService<T, TRepo>
    {
        IQueryable<T> GetAll();
        T GetById(Guid id);
        Task AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        Task<T> UpdateAsync(Guid id, T item);
        Task<bool> RemoveAsync(Guid id);
    }
}