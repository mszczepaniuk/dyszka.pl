using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        T GetById(Guid id);
        Task AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> items);
        Task<T> UpdateAsync(Guid id, T item);
        Task<bool> RemoveAsync(Guid id);
    }
}