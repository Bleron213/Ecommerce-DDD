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
                var products = (await _ecommerceDbContext.Products
                .Where(x => request.Request.OrderItems.Select(x => x.ProductId).Contains(x.Id))
                .ToListAsync())
                .Select(product => (product, request.Request.OrderItems.Where(y => y.ProductId == product.Id).Select(x => x.Quantity).First()));

                var order = await _ecommerceDbContext.Orders
                        .Include(x => x.OrderItems)
                            .ThenInclude(x => x.Product)
                        .Include(x => x.Customer)
                        .FirstOrDefaultAsync(x => x.Id == request.OrderId && !x.Deleted) ?? throw new AppException(API.Common.Errors.CoreErrors.GenericErrors.NotFound(nameof(Order)));

                order.ClearOrderItems();
                order.AddOrderItems(products);

                await _ecommerceDbContext.SaveChangesAsync();

                return order.ToOrderByIdResponse();
            }
        }
    }
}
