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
        protected readonly ApplicationDbContext context;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(T item)
        {
            await context.Set<T>().AddAsync(item);
            await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> items)
        {
            await context.Set<T>().AddRangeAsync(items);
            await context.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>().AsQueryable();
        }

        public T GetById(Guid id)
        {
            return context.Set<T>().FirstOrDefault(i => i.Id == id);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var entity = GetById(id);
            if (entity == null)
            {
                return false;
            }

            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<T> UpdateAsync(Guid id, T item)
        {
            item.Id = id;
            context.Set<T>().Update(item);
            await context.SaveChangesAsync();
            return item;
        }
    }
}
