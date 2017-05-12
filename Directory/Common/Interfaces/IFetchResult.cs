using System.Collections.Generic;

namespace Common
{
    public interface IFetchResult<T>
    {
        IEnumerable<T> Items { get; }
        int PageNumber { get; }
        int PagesCount { get; }
    }
}
