using System;
using System.Collections.Generic;

namespace CryptoCurrency.Core
{
    public class PagedCollection<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int ItemCount { get; set; }

        public int PageCount
        {
            get
            {
                if (PageSize == 0 && ItemCount > 0)
                    return 1;

                var pageCount = ItemCount / (double)PageSize;

                return (int)Math.Ceiling(pageCount);
            }
        }

        public ICollection<T> Items { get; set; }
    }
}
