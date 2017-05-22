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

        public Task Add(T entity)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task Add(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _items.Add(entity);
            }
            return Task.CompletedTask;
        }

        public Task Delete(T entity)
        {
            if (!_items.Remove(entity))
            {
                return Task.FromException(new DALMockException("Delete single entity exception"));
            }
            return Task.CompletedTask;
        }

        public Task Delete(IEnumerable<T> entities)
        {
            bool tempResult = true;
            foreach (var entity in entities)
            {
                tempResult &= _items.Remove(entity);
            }
            if (!tempResult)
            {
                return Task.FromException(new DALMockException("Delete many entities exception"));
            }
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            T entity = _items.FirstOrDefault(e => e.Id == id);
            if (_items.Remove(entity))
            {
                return Task.FromException(new DALMockException("Delete by id entity exception"));
            }
            return Task.CompletedTask;
        }

        public Task Delete(IEnumerable<int> ids)
        {
            bool tempResult = true;
            foreach (var id in ids)
            {
                T entity = _items.FirstOrDefault(e => e.Id == id);
                tempResult &= _items.Remove(entity);
            }
            if (!tempResult)
            {
                return Task.FromException(new DALMockException("Delete many entities by id exception"));
            }
            return Task.CompletedTask;
        }

        public Task<T> Get(int id)
        {
            T entity = _items.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(entity);
        }

        public Task<T> Get(Expression<Func<T, bool>> expression)
        {
            T entity = _items.FirstOrDefault(expression.Compile());
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return Task.FromResult<IEnumerable<T>>(_items);
        }

        public Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(_items.Where(expression.Compile()));
        }

        public Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> expression, IFetch fetch)
        {
            IEnumerable<T> entities = _items.Where(expression.Compile())
                .Skip(fetch.PageNumber * fetch.PageSize)
                .Take(fetch.PageSize);
            return Task.FromResult<IEnumerable<T>>(entities);
        }

        public Task Update(T entity)
        {
            T entityInCollection = _items.FirstOrDefault(e => e.Id == entity.Id);
            if (entityInCollection == null)
            {
                return Task.FromException(new DALMockException("Update entity not found"));
            }
            int index = _items.IndexOf(entityInCollection);
            _items.RemoveAt(index);
            _items.Insert(index, entity);
            return Task.CompletedTask;
        }

        public Task Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                T entityInCollection = _items.FirstOrDefault(e => e.Id == entity.Id);
                if (entityInCollection == null)
                {
                    return Task.FromException(new DALMockException("Update entity not found"));
                }
                int index = _items.IndexOf(entityInCollection);
                _items.RemoveAt(index);
                _items.Insert(index, entity);
            }
            return Task.CompletedTask;
        }
    }
}
