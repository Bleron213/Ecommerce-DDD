using Ecommerce.API.Common.Exceptions;
using Ecommerce.API.Contracts.Mapping;
using Ecommerce.API.Contracts.Request.Order;
using Ecommerce.API.Contracts.Response.Order;
using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Entities;
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
                if (!Guid.TryParse(_currentUserService.UserId, out var userGuid))
                    throw new AppException(new API.Common.Errors.CustomError(System.Net.HttpStatusCode.Forbidden, "not_allowed", "user not allowed"));

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    OrderDate = DateTimeOffset.UtcNow,
                    CustomerId = Guid.Parse(_currentUserService.UserId)
                };

                foreach (var orderItem in request.Request.OrderItems)
                {
                    order.OrderItems.Add(new Domain.Entities.OrderItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = orderItem.ProductId,
                        Quantity = orderItem.Quantity
                    });
                }

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
