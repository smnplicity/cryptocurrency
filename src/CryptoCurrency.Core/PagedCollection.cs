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
                return ItemCount / PageSize;
            }
        }

        public ICollection<T> Items { get; set; }
    }
}
