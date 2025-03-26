namespace Ecommerce.API.Common.Base
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalPages { get; set; }
        public T Items { get; set; }
        public PagedResponse(T data, int totalCount, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            Items = data;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalCount > 0 ? (int)Math.Ceiling(totalCount / (double)pageSize) : 0;
        }


        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            Items = data;
            PageSize = pageSize;
        }

        public PagedResponse()
        {
        }
    }

    public class Response<T> 
    {
        public Response(T items)
        {
            Items = items;
        }

        public T Items { get; set; }

    }
}
