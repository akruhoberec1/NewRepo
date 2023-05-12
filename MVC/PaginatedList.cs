using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC
{
    public class PaginatedList<T>
    {
        public int PageNum { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public int PageSize { get; private set; }   
        public List<T> Items { get; private set; }

        public PaginatedList(List<T> items, int count, int pageNum, int pageSize)
        {
            PageNum = pageNum;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
            PageSize = pageSize;
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNum > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNum < TotalPages);
            }
        }

        public static PaginatedList<T> Create(IQueryable<T> source, int pageNum, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageNum, pageSize);
        }
    }
}