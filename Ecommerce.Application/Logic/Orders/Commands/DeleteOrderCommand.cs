using Ecommerce.API.Common.Exceptions;
using Ecommerce.Application.Abstractions.Infrastructure;
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
    public class DeleteOrderCommand : IRequest
    {

        public DeleteOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }

        public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
        {
            private readonly IEcommerceDbContext _ecommerceDbContext;
            private readonly ICurrentUserService _currentUserService;

            public DeleteOrderCommandHandler(
                IEcommerceDbContext ecommerceDbContext,
                ICurrentUserService currentUserService
                )
            {
                _ecommerceDbContext = ecommerceDbContext;
                _currentUserService = currentUserService;
            }

            public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var order = await _ecommerceDbContext.Orders
                        .Include(x => x.OrderItems)
                            .ThenInclude(x => x.Product)
                        .Include(x => x.Customer)
                        .FirstOrDefaultAsync(x => x.Id == request.OrderId) ?? throw new AppException(API.Common.Errors.CoreErrors.GenericErrors.NotFound(nameof(Order)));

                order.CancelOrder();

                await _ecommerceDbContext.SaveChangesAsync();

            }
        }
    }
}
