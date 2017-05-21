using Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> Get(int id);
        Task<T> Get(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> expression, IFetch fetch);
        Task Update(T entity);
        Task Update(IEnumerable<T> entities);
        Task Add(T entity);
        Task Add(IEnumerable<T> entities);
        Task Delete(T entity);
        Task Delete(IEnumerable<T> entities);
        Task Delete(int id);
        Task Delete(IEnumerable<int> ids);
    }
}
