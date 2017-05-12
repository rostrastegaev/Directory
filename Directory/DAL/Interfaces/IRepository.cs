using Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<Result<T>> Get(int id);
        Task<Result<T>> Get(Expression<Func<T, bool>> expression);
        Task<Result<IEnumerable<T>>> GetAll();
        Task<Result<IEnumerable<T>>> GetMany(Expression<Func<T, bool>> expression);
        Task<Result<IFetchResult<T>>> GetMany(Expression<Func<T, bool>> expression, IFetch fetch);
        Task<Result> Update(T entity);
        Task<Result> Update(IEnumerable<T> entities);
        Task<Result> Add(T entity);
        Task<Result> Add(IEnumerable<T> entities);
        Task<Result> Delete(T entity);
        Task<Result> Delete(IEnumerable<T> entities);
        Task<Result> Delete(int id);
        Task<Result> Delete(IEnumerable<int> ids);
    }
}
