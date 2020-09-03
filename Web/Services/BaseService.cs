using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class BaseService<TItem, TRepo> : IBaseService<TItem, TRepo>
        where TItem : BaseEntity
        where TRepo : IBaseRepository<TItem>
    {
        private readonly TRepo repository;

        public BaseService(TRepo repository)
        {
            this.repository = repository;
        }

        public virtual IQueryable<TItem> GetAll()
        {
            return repository.GetAll();
        }

        public virtual TItem GetById(Guid id)
        {
            return repository.GetById(id);
        }

        public virtual Task AddAsync(TItem item)
        {
            return repository.AddAsync(item);
        }

        public virtual Task AddRangeAsync(IEnumerable<TItem> items)
        {
            return repository.AddRangeAsync(items);
        }

        public Task<TItem> UpdateAsync(Guid id, TItem item)
        {
            return repository.UpdateAsync(id, item);
        }

        public Task<bool> RemoveAsync(Guid id)
        {
            return repository.RemoveAsync(id);
        }
    }
}
