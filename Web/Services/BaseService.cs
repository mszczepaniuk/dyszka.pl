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
        protected readonly TRepo repository;
        protected readonly int resultsPerPage = 10;

        public BaseService(TRepo repository)
        {
            this.repository = repository;
        }

        public virtual IQueryable<TItem> GetAll()
        {
            return repository.GetAll();
        }

        public virtual PagedResult<TItem> GetPaged(int page)
        {
            return new PagedResult<TItem>
            {
                Items = repository.GetAll().Skip((page - 1) * resultsPerPage).Take(resultsPerPage).ToList(),
                CurrentPage = page,
                ResultsPerPage = resultsPerPage,
                PagesCount = repository.GetAll().Any() ?
                    (repository.GetAll().Count() - 1) / resultsPerPage + 1 :
                    0
            };
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

        public virtual Task<TItem> UpdateAsync(Guid id, TItem item)
        {
            return repository.UpdateAsync(id, item);
        }

        public virtual Task<bool> RemoveAsync(Guid id)
        {
            return repository.RemoveAsync(id);
        }
    }

    public class BaseService<T> : BaseService<T, IBaseRepository<T>>, IBaseService<T>
        where T : BaseEntity
    {
        public BaseService(IBaseRepository<T> repository) : base(repository)
        {

        }
    }
}
