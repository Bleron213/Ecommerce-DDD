using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.Shared.Attributes;
using Ecommerce.Domain.Entities.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        [TrackProperty]
        public int Quantity { get; private set; }
        [TrackProperty]
        public Price Price { get; private set; }

        public OrderItem(Product product, int quantity)
        {
            Id = Guid.NewGuid();
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be greater than 0.");
            ProductId = product.Id;
            SetOrderItemCurrentPrice();
        }

        private OrderItem()
        {
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public decimal GetTotalPrice() => Quantity * Product.Price.Amount;

        public void SetOrderItemCurrentPrice()
        {
            Price = new Price(GetTotalPrice());
        }

        #region Product
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }

        #endregion

        #region Order
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }

        #endregion
    }
}
