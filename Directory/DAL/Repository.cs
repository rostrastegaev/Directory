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

        public async Task<Result> Add(T entity)
        {
            await _set.AddAsync(entity);
            return Result.Success();
        }

        public async Task<Result> Add(IEnumerable<T> entities)
        {
            await _set.AddRangeAsync(entities);
            return Result.Success();
        }

        public async Task<Result> Delete(T entity)
        {
            await Task.Run(() => _set.Remove(entity));
            return Result.Success();
        }

        public async Task<Result> Delete(IEnumerable<T> entities)
        {
            await Task.Run(() => _set.RemoveRange(entities));
            return Result.Success();
        }

        public async Task<Result> Delete(int id)
        {
            await Task.Run(() => _set.Attach(new T { Id = id }).State = EntityState.Deleted);
            return Result.Success();
        }

        public async Task<Result> Delete(IEnumerable<int> ids)
        {
            await Task.Run(() =>
            {
                foreach (var id in ids)
                {
                    _set.Attach(new T { Id = id }).State = EntityState.Deleted;
                }
            });
            return Result.Success();
        }

        public async Task<Result<T>> Get(int id)
        {
            T entity = await _set.FirstOrDefaultAsync(e => e.Id.Equals(id));
            return Result<T>.Success(entity);
        }

        public async Task<Result<T>> Get(Expression<Func<T, bool>> expression)
        {
            T entity = await _set.FirstOrDefaultAsync(expression);
            return Result<T>.Success(entity);
        }

        public async Task<Result<IEnumerable<T>>> GetAll()
        {
            List<T> entities = await _set.ToListAsync();
            return Result<IEnumerable<T>>.Success(entities);
        }

        public async Task<Result<IEnumerable<T>>> GetMany(Expression<Func<T, bool>> expression)
        {
            List<T> entities = await _set.Where(expression).ToListAsync();
            return Result<IEnumerable<T>>.Success(entities);
        }

        public async Task<Result<IFetchResult<T>>> GetMany(Expression<Func<T, bool>> expression, IFetch fetch)
        {
            List<T> entities = await _set.Where(expression)
                .Skip(fetch.PageNumber * fetch.PageSize)
                .Take(fetch.PageSize)
                .ToListAsync();
            return Result<IFetchResult<T>>.Success(
                new FetchResult<T>(entities, fetch.PageNumber, fetch.PageSize));
        }

        public async Task<Result> Update(T entity)
        {
            await Task.Run(() => _set.Update(entity));
            return Result.Success();
        }

        public async Task<Result> Update(IEnumerable<T> entities)
        {
            await Task.Run(() => _set.UpdateRange(entities));
            return Result.Success();
        }
    }
}
