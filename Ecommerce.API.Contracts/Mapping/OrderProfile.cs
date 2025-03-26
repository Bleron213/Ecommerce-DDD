﻿using Ecommerce.API.Contracts.Response.Order;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.API.Contracts.Mapping
{
    public static class OrderProfile
    {
        public static OrderByIdResponse ToOrderByIdResponse(this Order order)
        {
            return new OrderByIdResponse
            {
                Id = order.Id,
                Customer = new API.Contracts.Response.Order.Customer
                {
                    Id = order.Customer.Id,
                    FirstName = order.Customer.FirstName,
                    LastName = order.Customer.LastName
                },
                TotalPrice = order.OrderItems.Sum(x => x.Quantity * x.Product.Price),
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(y => new Response.Order.OrderItem
                {
                    ProductId = y.Product.Id,
                    ProductName = y.Product.Name,
                    Quantity = y.Quantity,
                    Price = y.Product.Price
                }).ToList()
            };
        }

        public static OrderResponse ToOrderResponse(this Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                TotalPrice = order.OrderItems.Sum(x => x.Quantity * x.Product.Price),
                OrderDate = order.OrderDate
            };
        }
    }
}
