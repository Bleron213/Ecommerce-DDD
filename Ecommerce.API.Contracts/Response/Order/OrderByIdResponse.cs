using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.API.Contracts.Response.Order
{
    public class OrderByIdResponse
    {
        public Guid Id { get; set; }
        public required decimal TotalPrice { get; set; }
        public required DateTimeOffset OrderDate { get; set; }
        public required Customer Customer { get; set; }
        public required List<OrderItem> OrderItems = new List<OrderItem>();
    }

    public class Customer
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }

    public class OrderItem
    {
        public required string ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }
    }
}
