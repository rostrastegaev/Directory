using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class FetchResult<T> : IFetchResult<T>
    {
        public IEnumerable<T> Items { get; }
        public int PageNumber { get; }
        public int PagesCount { get; }

        public FetchResult(IEnumerable<T> items, int pageNumber, int pageSize)
        {
            Items = items;
            PageNumber = pageNumber;
            PagesCount = (int)Math.Ceiling((double)items?.Count() / pageSize);
        }
    }
}
