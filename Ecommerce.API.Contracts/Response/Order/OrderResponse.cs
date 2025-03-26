using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.API.Contracts.Response.Order
{
    public class OrderResponse
    {
        public required Guid Id { get; set; }
        public required decimal TotalPrice { get; set; }
        public required DateTimeOffset OrderDate { get; set; }
        public required OrderStatus OrderStatus { get; set; }

    }

    public enum OrderStatus 
    {
        PENDING,
        PROCESSING,
        SHIPPED,
        DELIVERED,
        CANCELLED,
        REFUNDED
    }
}
