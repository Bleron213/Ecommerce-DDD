using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product(string name, Price price)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(name);

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public Price Price { get; private set; }


        
    }


}
