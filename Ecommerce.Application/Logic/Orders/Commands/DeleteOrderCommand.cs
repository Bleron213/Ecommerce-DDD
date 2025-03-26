using Ecommerce.API.Common.Exceptions;
using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Repositories;
using Ecommerce.Domain.Specifications;
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
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICurrentUserService _currentUserService;

            public DeleteOrderCommandHandler(
                IUnitOfWork unitOfWork,
                ICurrentUserService currentUserService
                )
            {
                _unitOfWork = unitOfWork;
                _currentUserService = currentUserService;
            }


            public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(request.OrderId)) ?? throw new AppException(API.Common.Errors.CoreErrors.GenericErrors.NotFound(nameof(Order)));

                order.CancelOrder();

                await _unitOfWork.Complete();

            }
        }
    }
}
