using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Shared.ValueObjects
{
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
