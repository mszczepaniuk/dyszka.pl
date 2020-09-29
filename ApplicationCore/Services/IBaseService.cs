using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;

namespace ApplicationCore.Services
{
    public interface IBaseService<T, TRepo>
    {
        IQueryable<T> GetAll();
        T GetById(Guid id);
        Task<Guid> AddAsync(T item);
        Task<IEnumerable<Guid>> AddRangeAsync(IEnumerable<T> items);
        Task<T> UpdateAsync(Guid id, T item);
        Task<bool> RemoveAsync(Guid id);
    }

    public interface IBaseService<T> : IBaseService<T, IBaseRepository<T>>
        where T : BaseEntity
    {

    }

    public interface IExtendedBaseService<T, TRepo> : IBaseService<T, TRepo>
    {
        PagedResult<T> GetPaged(int page);
        public PagedResult<TVm> GetPagedAndMapTo<TVm>(int page);
    }

    public interface IExtendedBaseService<T> : IExtendedBaseService<T, IBaseRepository<T>>
        where T : BaseEntity
    {
        
    }
}