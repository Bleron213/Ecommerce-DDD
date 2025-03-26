using Ecommerce.API.Common.Base;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Infrastructure.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(Guid id, PaginationFilterRequest request) : base(x => x.CustomerId == id)
        {

            AddInclude(x => x.OrderItems);
            AddInclude($"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}");
            AddInclude(x => x.Customer);
            ApplyPaging((request.PageNumber - 1) * request.PageSize, request.PageSize);

            if (!string.IsNullOrEmpty(request.OrderByField))
            {
                if (request.OrderByField.Equals("orderdate", StringComparison.OrdinalIgnoreCase))
                {
                    if (request.OrderBy == SortBy.Ascending)
                    {
                        AddOrderBy(x => x.OrderDate);
                    }
                    else
                    {
                        AddOrderByDescending(x => x.OrderDate);
                    }
                }
            }
        }

    }

    public class OrdersByCustomerIdSpecification : BaseSpecification<Order>
    {
        public OrdersByCustomerIdSpecification(Guid id) : base(x => x.CustomerId == id)
        {

        }

    }


}
