using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext dbContext;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual async Task AddAsync(T item)
        {
            await dbContext.Set<T>().AddAsync(item);
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> items)
        {
            await dbContext.Set<T>().AddRangeAsync(items);
            await dbContext.SaveChangesAsync();
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbContext.Set<T>().AsQueryable();
        }

        public virtual T GetById(Guid id)
        {
            return dbContext.Set<T>().FirstOrDefault(i => i.Id == id);
        }

        public virtual async Task<bool> RemoveAsync(Guid id)
        {
            var entity = GetById(id);
            if (entity == null)
            {
                return false;
            }

            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<T> UpdateAsync(Guid id, T item)
        {
            item.Id = id;
            dbContext.Set<T>().Update(item);
            await dbContext.SaveChangesAsync();
            return item;
        }
    }
}
