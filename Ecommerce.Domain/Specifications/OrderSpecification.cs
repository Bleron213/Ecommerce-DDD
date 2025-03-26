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
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(Guid id) : base(x => x.Id == id)
        {

            AddInclude(x => x.OrderItems);
            AddInclude($"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}");
            AddInclude(x => x.Customer);
            
        }
    }
}
