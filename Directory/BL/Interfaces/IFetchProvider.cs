using Common;
using System;
using System.Linq.Expressions;
using DAL;

namespace BL
{
    public interface IFetchProvider<T> where T : class, IEntity
    {
        Expression<Func<T, bool>> Compile(IFetch fetch, int userId);
    }
}
