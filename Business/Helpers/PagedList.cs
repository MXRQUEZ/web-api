using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Helpers
{
    public sealed class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var sourceList = source.ToList();
            var items = sourceList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            TotalCount = sourceList.Count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            AddRange(items);
        }

    }
}
