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
    public class EditOrderCommand : IRequest<OrderByIdResponse>
    {
        public EditOrderCommand(Guid orderId, EditOrderRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            OrderId = orderId;
            Request = request;
        }

        public Guid OrderId { get; }
        public EditOrderRequest Request { get; }

        public class EditOrderCommandHandler : IRequestHandler<EditOrderCommand, OrderByIdResponse>
        {
            private readonly IEcommerceDbContext _ecommerceDbContext;
            private readonly ICurrentUserService _currentUserService;

            public EditOrderCommandHandler(
                IEcommerceDbContext ecommerceDbContext,
                ICurrentUserService currentUserService
                )
            {
                _ecommerceDbContext = ecommerceDbContext;
                _currentUserService = currentUserService;
            }

            public async Task<OrderByIdResponse> Handle(EditOrderCommand request, CancellationToken cancellationToken)
            {
                if (!Guid.TryParse(_currentUserService.UserId, out var userGuid))
                    throw new AppException(new API.Common.Errors.CustomError(System.Net.HttpStatusCode.Forbidden, "not_allowed", "user not allowed"));

                var order = await _ecommerceDbContext.Orders
                        .Include(x => x.OrderItems)
                            .ThenInclude(x => x.Product)
                        .Include(x => x.Customer)
                        .FirstOrDefaultAsync(x => x.Id == request.OrderId && !x.Deleted) ?? throw new AppException(API.Common.Errors.CoreErrors.GenericErrors.NotFound(nameof(Order)));

                order.OrderItems.Clear();

                foreach (var orderItem in request.Request.OrderItems)
                {
                    order.OrderItems.Add(new Domain.Entities.OrderItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = orderItem.ProductId,
                        Quantity = orderItem.Quantity
                    });
                }

                await _ecommerceDbContext.SaveChangesAsync();

                return order.ToOrderByIdResponse();
            }
        }
    }
}
