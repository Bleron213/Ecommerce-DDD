using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Customer(string firstName, string lastName, Address address)
        {
            ArgumentException.ThrowIfNullOrEmpty(firstName);
            ArgumentException.ThrowIfNullOrEmpty(lastName);
            ArgumentNullException.ThrowIfNull(address);

            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Address = address;
        }

        public Customer(Guid id, string firstName, string lastName, Address address)
        {
            ArgumentException.ThrowIfNullOrEmpty(firstName);
            ArgumentException.ThrowIfNullOrEmpty(lastName);
            ArgumentNullException.ThrowIfNull(address);

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
        }

        public void AddOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);

            Orders.Add(order);
        }

        public void RemoveOrder(Guid orderId)
        {
            var orderToRemove = Orders.FirstOrDefault(x => x.Id == orderId);

            if (orderToRemove != null)
                Orders.Remove(orderToRemove);
        }

        [TrackProperty]
        public string FirstName { get; private set; }
        [TrackProperty]
        public string LastName { get; private set; }
        [TrackProperty]
        public Address Address { get; private set; }

        #region Navigation
        public List<Order> Orders { get; private set; } = [];
        #endregion
    }

    public class Address
    {
        public Address(string street, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty.", nameof(street));

            if (string.IsNullOrWhiteSpace(postalCode))
                throw new ArgumentException("PostalCode cannot be empty.", nameof(postalCode));

            Street = street;
            PostalCode = postalCode;
        }

        public string Street { get; }
        public string PostalCode { get; }

        public override bool Equals(object? obj)
        {
            return obj is Address address &&
                   Street == address.Street &&
                   PostalCode == address.PostalCode;
        }
        public override int GetHashCode() => HashCode.Combine(Street, PostalCode);

    }
}
