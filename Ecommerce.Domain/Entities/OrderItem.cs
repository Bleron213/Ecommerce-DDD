using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Quantity { get; private set; }

        public OrderItem(Product product, int quantity)
        {
            Id = Guid.NewGuid();
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be greater than 0.");

            ProductId = product.Id;
        }

        public OrderItem(Guid productId, int quantity)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be greater than 0.");
        }

        public decimal GetTotalPrice() => Quantity * Product.Price.Amount;

        #region Product
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        #endregion

        #region Order
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        #endregion
    }
}
