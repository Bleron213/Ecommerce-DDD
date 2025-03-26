using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.API.Common.Base
{
    public class PaginationFilterRequest
    {
        private int _pageNumber = 1;

        public int PageNumber
        {
            get
            {
                return _pageNumber <= 0 ? 1 : _pageNumber;
            }
            set
            {
                _pageNumber = value;
            }
        }

        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = "";
        public string OrderByField { get; set; } = string.Empty;
        public SortBy OrderBy { get; set; } = SortBy.Descending;
    }

    public enum SortBy
    {
        Ascending = 0,
        Descending = 1
    }
}
