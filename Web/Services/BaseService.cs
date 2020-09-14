using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using AutoMapper;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class BaseService<TItem, TRepo> : IBaseService<TItem, TRepo>
        where TItem : BaseEntity
        where TRepo : IBaseRepository<TItem>
    {
        protected readonly TRepo repository;

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

    public class ExtendedBaseService<TItem, TRepo> : BaseService<TItem, TRepo>, IExtendedBaseService<TItem, TRepo>
        where TItem : BaseEntity
        where TRepo : IBaseRepository<TItem>
    {
        private readonly IMapper mapper;
        protected readonly int resultsPerPage = 10;

        public ExtendedBaseService(TRepo repository, IMapper mapper) : base(repository)
        {
            this.mapper = mapper;
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

        public virtual PagedResult<TVm> GetPagedAndMapTo<TVm>(int page)
        {
            return new PagedResult<TVm>
            {
                Items = mapper.Map<List<TVm>>(repository.GetAll().Skip((page - 1) * resultsPerPage).Take(resultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = resultsPerPage,
                PagesCount = repository.GetAll().Any() ?
                    (repository.GetAll().Count() - 1) / resultsPerPage + 1 :
                    0
            };
        }
    }

    public class ExtendedBaseService<T> : ExtendedBaseService<T, IBaseRepository<T>>, IExtendedBaseService<T>
        where T : BaseEntity
    {
        public ExtendedBaseService(IBaseRepository<T> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
