using Ecommerce.API.Common.Base;
using Ecommerce.API.Common.Exceptions;
using Ecommerce.API.Contracts.Mapping;
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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Logic.Orders.Query
{
    public class GetOrdersQuery : IRequest<PagedResponse<List<OrderResponse>>>
    {
        public GetOrdersQuery(PaginationFilterRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            Request = request;
        }

        public PaginationFilterRequest Request { get; }

        public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PagedResponse<List<OrderResponse>>>
        {
            private readonly IEcommerceDbContext _ecommerceDbContext;
            private readonly ICurrentUserService _currentUserService;
            private readonly IUnitOfWork _unitOfWork;

            public GetOrdersQueryHandler(
                IEcommerceDbContext ecommerceDbContext,
                ICurrentUserService currentUserService,
                IUnitOfWork unitOfWork
                )
            {
                _ecommerceDbContext = ecommerceDbContext;
                _currentUserService = currentUserService;
                _unitOfWork = unitOfWork;
            }

            public async Task<PagedResponse<List<OrderResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            {
                var orders = (await _unitOfWork.Repository<Order>().ListAsync(new OrdersWithItemsAndOrderingSpecification(_currentUserService.UserGuid, request.Request)))
                    .Select(x => x.ToOrderResponse())
                    .ToList();

                var count = await _unitOfWork.Repository<Order>().CountAsync(new OrdersByCustomerIdSpecification(_currentUserService.UserGuid));

                return new PagedResponse<List<OrderResponse>>(orders, orders.Count, request.Request.PageNumber, request.Request.PageSize);
            }
        }
    }
}
