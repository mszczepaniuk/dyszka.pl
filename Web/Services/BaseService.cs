﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using AutoMapper;

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

        public virtual Task<Guid> AddAsync(TItem item)
        {
            return repository.AddAsync(item);
        }

        public virtual Task<IEnumerable<Guid>> AddRangeAsync(IEnumerable<TItem> items)
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
        protected readonly IMapper mapper;
        protected virtual int ResultsPerPage => 10;

        public ExtendedBaseService(TRepo repository, IMapper mapper) : base(repository)
        {
            this.mapper = mapper;
        }

        public virtual PagedResult<TItem> GetPaged(int page)
        {
            return new PagedResult<TItem>
            {
                Items = repository.GetAll().Skip((page - 1) * ResultsPerPage).Take(ResultsPerPage).ToList(),
                CurrentPage = page,
                ResultsPerPage = ResultsPerPage,
                PagesCount = repository.GetAll().Any() ?
                    (repository.GetAll().Count() - 1) / ResultsPerPage + 1 :
                    0
            };
        }

        public virtual PagedResult<TVm> GetPagedAndMapTo<TVm>(int page)
        {
            return new PagedResult<TVm>
            {
                Items = mapper.Map<List<TVm>>(repository.GetAll().Skip((page - 1) * ResultsPerPage).Take(ResultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = ResultsPerPage,
                PagesCount = repository.GetAll().Any() ?
                    (repository.GetAll().Count() - 1) / ResultsPerPage + 1 :
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
