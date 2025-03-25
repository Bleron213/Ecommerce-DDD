using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.API.Contracts.Request.Order
{
    public class CreateOrderRequest
    {
        public required List<OrderItemRequest> OrderItems { get; set; } = [];
    }
}
