using Ecommerce.API.Common.Exceptions;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Ecommerce.Domain.Entities.Shared.ValueObjects;
using NUnit.Framework.Legacy;

namespace Ecommerce.Tests.UnitTests.Entities
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void Constructor_ValidParameters_CreatesOrder()
        {
            // Arrange
            var orderDate = DateTimeOffset.UtcNow;
            var customerId = Guid.NewGuid();

            // Act
            var order = new Order(orderDate, customerId);

            // ClassicAssert
            Assert.That(order.OrderDate, Is.EqualTo(orderDate));
            Assert.That(order.CustomerId, Is.EqualTo(customerId));
            Assert.That(order.Status, Is.EqualTo(OrderStatus.PENDING));
        }

        [Test]
        public void CancelOrder_ValidOrder_CancelsOrder()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());

            // Act
            order.CancelOrder();

            // ClassicAssert
            Assert.That(order.Status, Is.EqualTo(OrderStatus.CANCELLED));
        }

        [Test]
        public void AddOrderItem_ValidOrder_AddsOrderItem()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());
            var product = new Product( "Product 1", new Domain.Entities.Shared.ValueObjects.Price(10));
            var quantity = 2;

            // Act
            order.AddOrderItem(product, quantity);

            // ClassicAssert
            Assert.That(order.OrderItems.Count, Is.EqualTo(1));
            Assert.That(order.OrderItems[0].Quantity, Is.EqualTo(quantity));
        }

        [Test]
        public void AddOrderItem_ZeroOrNegativeQuantity_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());
            var product = new Product( "Product 1", new Domain.Entities.Shared.ValueObjects.Price(10));

            // Act & ClassicAssert
            var ex = Assert.Throws<ArgumentException>(() => order.AddOrderItem(product, 0));
            Assert.That(ex.Message, Is.EqualTo("Quantity must be greater than 0. (Parameter 'quantity')"));
        }

        [Test]
        public void AddOrderItems_ValidItems_AddsMultipleOrderItems()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());
            var products = new List<(Product, int)>
            {
                (new Product("Product 1", new Price(10)), 1),
                (new Product("Product 2", new Price(15)), 2)
            };

            // Act
            order.AddOrderItems(products);

            // ClassicAssert
            Assert.That(order.OrderItems.Count, Is.EqualTo(2));
        }

        [Test]
        public void ModifyOrderItems_ValidItems_ModifiesOrderItem()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());
            var product = new Product("Product 1", new Price(10));
            order.AddOrderItem(product, 1);

            var updatedItems = new List<(Product, int)>
            {
                (product, 5)
            };

            // Act
            order.ModifyOrderItems(updatedItems);

            // ClassicAssert
            Assert.That(order.OrderItems[0].Quantity, Is.EqualTo(5));
        }

        [Test]
        public void ModifyOrderItems_QuantityLessThanZero_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());
            var product = new Product("Product 1", new Price(10));
            order.AddOrderItem(product, 1);

            // Act & ClassicAssert
            var ex = Assert.Throws<ArgumentException>(() => order.ModifyOrderItems(new List<(Product, int)> { (product, -1) }));
            Assert.That(ex.Message, Is.EqualTo("Quantity must be greater than 0. (Parameter 'Quantity')"));
        }

        [Test]
        public void UpdateOrderStatus_ValidStatusTransition_UpdatesStatus()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());

            // Act
            order.UpdateOrderStatus(OrderStatus.PROCESSING);

            // ClassicAssert
            Assert.That(order.Status, Is.EqualTo(OrderStatus.PROCESSING));
        }

        [Test]
        public void UpdateOrderStatus_InvalidStatusTransition_ThrowsAppException()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());
            order.UpdateOrderStatus(OrderStatus.PROCESSING);

            // Act & ClassicAssert
            var ex = ClassicAssert.Throws<AppException>(() => order.UpdateOrderStatus(OrderStatus.DELIVERED));
            Assert.That(ex.Message, Is.EqualTo("Invalid status move from PROCESSING to DELIVERED"));
        }

        [Test]
        public void Editable_StatusIsPending_ReturnsTrue()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());

            // Act
            var isEditable = order.Editable();

            // ClassicAssert
            Assert.That(isEditable, Is.True);
        }

        [Test]
        public void Editable_StatusIsShipped_ReturnsFalse()
        {
            // Arrange
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());

            order.UpdateOrderStatus(OrderStatus.PROCESSING);
            order.UpdateOrderStatus(OrderStatus.SHIPPED);


            // Act
            var isEditable = order.Editable();

            // ClassicAssert

            Assert.That(isEditable, Is.False);
        }

        [Test]
        public void UpdateOrderStatus_ValidAndInvalidTransitions_BehaveAsExpected()
        {
            var order = new Order(DateTimeOffset.UtcNow, Guid.NewGuid());

            // Valid: PENDING → PROCESSING
            order.UpdateOrderStatus(OrderStatus.PROCESSING);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.PROCESSING));

            // Valid: PROCESSING → SHIPPED
            order.UpdateOrderStatus(OrderStatus.SHIPPED);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.SHIPPED));

            // Valid: SHIPPED → DELIVERED
            order.UpdateOrderStatus(OrderStatus.DELIVERED);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.DELIVERED));

            // Invalid: DELIVERED → CANCELLED
            var ex1 = Assert.Throws<AppException>(() => order.UpdateOrderStatus(OrderStatus.CANCELLED));
            Assert.That(ex1.Message, Is.EqualTo("Invalid status move from DELIVERED to CANCELLED"));

            // Invalid: DELIVERED → PROCESSING
            var ex2 = Assert.Throws<AppException>(() => order.UpdateOrderStatus(OrderStatus.PROCESSING));
            Assert.That(ex2.Message, Is.EqualTo("Invalid status move from DELIVERED to PROCESSING"));
        }

    }
}
