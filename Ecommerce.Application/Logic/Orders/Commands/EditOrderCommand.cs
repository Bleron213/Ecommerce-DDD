using Ecommerce.API.Common.Exceptions;
using Ecommerce.API.Contracts.Mapping;
using Ecommerce.API.Contracts.Request.Order;
using Ecommerce.API.Contracts.Response.Order;
using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Entities;
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
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICurrentUserService _currentUserService;

            public EditOrderCommandHandler(
                IUnitOfWork unitOfWork,
                ICurrentUserService currentUserService
                )
            {
                _unitOfWork = unitOfWork;
                _currentUserService = currentUserService;
            }

            public async Task<OrderByIdResponse> Handle(EditOrderCommand request, CancellationToken cancellationToken)
            {
                var products = (await _unitOfWork.Repository<Product>().ListAsync(new ProductSpecification(request.Request.OrderItems.Select(x => x.ProductId).ToList())))
                        .Select(product => (product, request.Request.OrderItems.Where(y => y.ProductId == product.Id).Select(x => x.Quantity).First()));

                var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(request.OrderId)) ?? throw new AppException(API.Common.Errors.CoreErrors.GenericErrors.NotFound(nameof(Order)));

                if (!order.Editable())
                    throw new AppException(new API.Common.Errors.CustomError("order_cannot_be_edited", "Order cannot be edited at this state"));

                order.ModifyOrderItems(products);

                await _unitOfWork.Complete();

                return order.ToOrderByIdResponse();
            }
        }
    }
}
