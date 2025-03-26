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
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICurrentUserService _currentUserService;

            public CreateOrderCommandHandler(
                IUnitOfWork unitOfWork,
                ICurrentUserService currentUserService
                )
            {
                _unitOfWork = unitOfWork;
                _currentUserService = currentUserService;
            }

            public async Task<OrderByIdResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var products = (await _unitOfWork.Repository<Product>().ListAsync(new ProductSpecification(request.Request.OrderItems.Select(x => x.ProductId).ToList())))
                    .Select(product => (product, request.Request.OrderItems.Where(y => y.ProductId == product.Id).Select(x => x.Quantity).First()));

                var order = new Order(DateTimeOffset.UtcNow, _currentUserService.UserGuid);

                order.AddOrderItems(products);

                await _unitOfWork.Repository<Order>().Add(order);
                await _unitOfWork.Complete();

                var insertedOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(order.Id));

                return insertedOrder!.ToOrderByIdResponse();
            }
        }
    }
}
