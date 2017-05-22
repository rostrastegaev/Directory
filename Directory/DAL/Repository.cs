using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private DbSet<T> _set;

        public Repository(DbSet<T> set)
        {
            _set = set;
        }

        public async Task Add(T entity)
        {
            await _set.AddAsync(entity);
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await _set.AddRangeAsync(entities);
        }

        public async Task Delete(T entity)
        {
            await Task.Run(() => _set.Remove(entity));
        }

        public async Task Delete(IEnumerable<T> entities)
        {
            await Task.Run(() => _set.RemoveRange(entities));
        }

        public async Task Delete(int id)
        {
            await Task.Run(() => _set.Attach(new T { Id = id }).State = EntityState.Deleted);
        }

        public async Task Delete(IEnumerable<int> ids)
        {
            await Task.Run(() =>
            {
                foreach (var id in ids)
                {
                    _set.Attach(new T { Id = id }).State = EntityState.Deleted;
                }
            });
        }

        public async Task<T> Get(int id)
        {
            T entity = await _set.FirstOrDefaultAsync(e => e.Id.Equals(id));
            return entity;
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression)
        {
            T entity = await _set.FirstOrDefaultAsync(expression);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            List<T> entities = await _set.ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> expression)
        {
            List<T> entities = await _set.Where(expression).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> expression, IFetch fetch)
        {
            List<T> entities = await _set.Where(expression)
                .Skip(fetch.PageNumber * fetch.PageSize)
                .Take(fetch.PageSize)
                .ToListAsync();
            return entities;
        }

        public async Task Update(T entity)
        {
            await Task.Run(() => _set.Update(entity));
        }

        public async Task Update(IEnumerable<T> entities)
        {
            await Task.Run(() => _set.UpdateRange(entities));
        }
    }
}
