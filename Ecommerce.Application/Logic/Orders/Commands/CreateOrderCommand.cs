using Ecommerce.API.Common.Exceptions;
using Ecommerce.API.Contracts.Mapping;
using Ecommerce.API.Contracts.Request.Order;
using Ecommerce.API.Contracts.Response.Order;
using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Logic.Orders.Commands
{
    public class CreateOrderCommand : IRequest<OrderByIdResponse>
    {
        public CreateOrderCommand(CreateOrderRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            Request = request;
        }

        public CreateOrderRequest Request { get; }

        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderByIdResponse>
        {
            private readonly IEcommerceDbContext _ecommerceDbContext;
            private readonly ICurrentUserService _currentUserService;

            public CreateOrderCommandHandler(
                IEcommerceDbContext ecommerceDbContext,
                ICurrentUserService currentUserService
                )
            {
                _ecommerceDbContext = ecommerceDbContext;
                _currentUserService = currentUserService;
            }

            public async Task<OrderByIdResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var products = (await _ecommerceDbContext.Products
                    .Where(x => request.Request.OrderItems.Select(x => x.ProductId).Contains(x.Id))
                    .ToListAsync())
                    .Select(product => (product, request.Request.OrderItems.Where(y => y.ProductId == product.Id).Select(x => x.Quantity).First()));

                var order = new Order(DateTimeOffset.UtcNow, _currentUserService.UserGuid);

                order.AddOrderItems(products);

                await _ecommerceDbContext.Orders.AddAsync(order);

                await _ecommerceDbContext.SaveChangesAsync();

                var insertedOrder = await _ecommerceDbContext.Orders
                        .Include(x => x.OrderItems)
                            .ThenInclude(x => x.Product)
                        .Include(x => x.Customer)
                        .FirstAsync(x => x.Id == order.Id);

                return insertedOrder.ToOrderByIdResponse();
            }
        }
    }
}
