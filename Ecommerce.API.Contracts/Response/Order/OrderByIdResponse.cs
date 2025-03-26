using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.API.Contracts.Response.Order
{
    public class OrderByIdResponse
    {
        public required Guid Id { get; set; }
        public required decimal TotalPrice { get; set; }
        public required DateTimeOffset OrderDate { get; set; }
        public required Customer Customer { get; set; }
        public required List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public required OrderStatus OrderStatus { get; set; }
    }

    public class Customer
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }

    public class OrderItem
    {
        public required Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required Price Price { get; set; }
    }

    public class Price
    {
        public decimal Amount { get; }

        public Price(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(amount));

            Amount = amount;
        }

        public override bool Equals(object? obj)
        {
            return obj is Price price &&
                   Amount == price.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount);
        }
    }
}
