using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order(DateTimeOffset orderDate, Guid customerId)
        {
            Id = Guid.NewGuid();
            OrderDate = orderDate;
            CustomerId = customerId;
            Deleted = false; 
        }

        public Price TotalPrice { get; private set; }
        public DateTimeOffset OrderDate { get; private set; }
        public bool Deleted { get; private set; }

        public void MarkAsDeleted()
        {
            Deleted = true;
        }

        public void ClearOrderItems()
        {
            OrderItems.Clear();
        }

        public void AddOrderItem(Product product, int quantity)
        {
            ArgumentNullException.ThrowIfNull(product);
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than 0.", nameof(quantity));

            OrderItems.Add(new OrderItem(product, quantity));

            RecalculateTotalPrice();
        }

        public void AddOrderItems(IEnumerable<(Product Product, int Quantity)> Products)
        {
            foreach (var product in Products)
            {
                if (product.Quantity <= 0) throw new ArgumentException("Quantity must be greater than 0.", nameof(product.Quantity));
                OrderItems.Add(new OrderItem(product.Product, product.Quantity));
            }

            RecalculateTotalPrice();
        }

        private void RecalculateTotalPrice()
        {
            TotalPrice = new Price(OrderItems.Sum(item => item.GetTotalPrice()));
        }

        #region Customer
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        #endregion

        #region Navigations
        public List<OrderItem> OrderItems { get; set; } = [];

        #endregion


    }

    
}
