using Ecommerce.API.Common.Errors;
using Ecommerce.API.Common.Exceptions;
using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities.Shared.Attributes;
using Ecommerce.Domain.Entities.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        public Order(DateTimeOffset orderDate, Guid customerId)
        {
            Id = Guid.NewGuid();
            OrderDate = orderDate;
            CustomerId = customerId;
            Status = OrderStatus.PENDING;
        }

        [TrackProperty]
        public Price TotalPrice { get; private set; }
        public DateTimeOffset OrderDate { get; private set; }
        [TrackProperty]
        public OrderStatus Status { get; private set; }

        public void CancelOrder()
        {
            UpdateOrderStatus(OrderStatus.CANCELLED);
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

        public void ModifyOrderItems(IEnumerable<(Product Product, int Quantity)> Products)
        {
            foreach (var product in Products)
            {
                if (product.Quantity <= 0) throw new ArgumentException("Quantity must be greater than 0.", nameof(product.Quantity));

                var orderItem = OrderItems.Where(x => x.ProductId == product.Product.Id).FirstOrDefault();

                if(orderItem != null)
                {
                    orderItem.SetQuantity(product.Quantity);
                    orderItem.SetOrderItemCurrentPrice();
                } 
                else
                {
                    OrderItems.Add(new OrderItem(product.Product, product.Quantity));
                }
            }

            RecalculateTotalPrice();

        }

        private void RecalculateTotalPrice()
        {
            TotalPrice = new Price(OrderItems.Sum(item => item.GetTotalPrice()));
        }

        public void UpdateOrderStatus(OrderStatus newStatus)
        {
            if (!ValidateStatusTransition(newStatus))
                throw new AppException(new CustomError("invalid_status_move", $"Invalid status move from {Status} to {newStatus}"));

            Status = newStatus;
        }

        private bool ValidateStatusTransition(OrderStatus newStatus)
        {
            return Status switch
            {
                OrderStatus.PENDING => newStatus == OrderStatus.PROCESSING || newStatus == OrderStatus.CANCELLED,
                OrderStatus.PROCESSING => newStatus == OrderStatus.SHIPPED || newStatus == OrderStatus.CANCELLED,
                OrderStatus.SHIPPED => newStatus == OrderStatus.DELIVERED || newStatus == OrderStatus.CANCELLED,

                // once an order is delivered, shipped or refunded it cannot move statuses any further.
                OrderStatus.DELIVERED => false,
                OrderStatus.CANCELLED => false,
                OrderStatus.REFUNDED => false,
                _ => false,
            };
        }

        public bool Editable()
        {
            return Status switch
            {
                OrderStatus.PENDING => true,
                OrderStatus.PROCESSING => true,
                _ => false,
            };
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
