using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities.Shared.Attributes;
using Ecommerce.Domain.Entities.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        private Product()
        {
        }

        public Product(string name, Price price)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(name);

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }

        [TrackProperty]
        public string Name { get; private set; }
        [TrackProperty]
        public Price Price { get; private set; }
    }
}
