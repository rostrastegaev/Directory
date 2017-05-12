using DAL;
using System;
using System.Collections.Generic;
using System.Text;
using Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace Tests.DAL
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private IList<T> _items;

        public Repository(IList<T> items)
        {
            _items = items;
        }

        public Task<Result> Add(T entity)
        {
            _items.Add(entity);
            return Task.FromResult(Result.Success());
        }

        public Task<Result> Add(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _items.Add(entity);
            }
            return Task.FromResult(Result.Success());
        }

        public Task<Result> Delete(T entity)
        {
            Result result = _items.Remove(entity) ? Result.Success() : Result.Error(1);
            return Task.FromResult(result);
        }

        public Task<Result> Delete(IEnumerable<T> entities)
        {
            bool tempResult = true;
            foreach (var entity in entities)
            {
                tempResult &= _items.Remove(entity);
            }
            return Task.FromResult(tempResult ? Result.Success() : Result.Error(1));
        }

        public Task<Result> Delete(int id)
        {
            T entity = _items.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(_items.Remove(entity) ? Result.Success() : Result.Error(1));
        }

        public Task<Result> Delete(IEnumerable<int> ids)
        {
            bool tempResult = true;
            foreach (var id in ids)
            {
                T entity = _items.FirstOrDefault(e => e.Id == id);
                tempResult &= _items.Remove(entity);
            }
            return Task.FromResult(tempResult ? Result.Success() : Result.Error(1));
        }

        public Task<Result<T>> Get(int id)
        {
            T entity = _items.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(Result<T>.Success(entity));
        }

        public Task<Result<T>> Get(Expression<Func<T, bool>> expression)
        {
            T entity = _items.FirstOrDefault(expression.Compile());
            return Task.FromResult(Result<T>.Success(entity));
        }

        public Task<Result<IEnumerable<T>>> GetAll()
        {
            return Task.FromResult(Result<IEnumerable<T>>.Success(_items));
        }

        public Task<Result<IEnumerable<T>>> GetMany(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(Result<IEnumerable<T>>.Success(_items.Where(expression.Compile())));
        }

        public Task<Result<IFetchResult<T>>> GetMany(Expression<Func<T, bool>> expression, IFetch fetch)
        {
            IEnumerable<T> entities = _items.Where(expression.Compile())
                .Skip(fetch.PageNumber * fetch.PageSize)
                .Take(fetch.PageSize);
            return Task.FromResult(Result<IFetchResult<T>>.Success(
                new FetchResult<T>(entities, fetch.PageNumber, fetch.PageSize)));
        }

        public Task<Result> Update(T entity)
        {
            T entityInCollection = _items.FirstOrDefault(e => e.Id == entity.Id);
            if (entityInCollection == null)
            {
                return Task.FromResult(Result.Error(1));
            }
            int index = _items.IndexOf(entityInCollection);
            _items.RemoveAt(index);
            _items.Insert(index, entity);
            return Task.FromResult(Result.Success());
        }

        public Task<Result> Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                T entityInCollection = _items.FirstOrDefault(e => e.Id == entity.Id);
                if (entityInCollection == null)
                {
                    return Task.FromResult(Result.Error(1));
                }
                int index = _items.IndexOf(entityInCollection);
                _items.RemoveAt(index);
                _items.Insert(index, entity);
            }
            return Task.FromResult(Result.Success());
        }
    }
}
