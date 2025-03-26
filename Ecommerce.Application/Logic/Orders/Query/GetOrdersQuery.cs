using Ecommerce.API.Common.Base;
using Ecommerce.API.Common.Exceptions;
using Ecommerce.API.Contracts.Mapping;
using Ecommerce.API.Contracts.Response.Order;
using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Domain.Entities;
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

            public GetOrdersQueryHandler(
                IEcommerceDbContext ecommerceDbContext,
                ICurrentUserService currentUserService
                )
            {
                _ecommerceDbContext = ecommerceDbContext;
                _currentUserService = currentUserService;
            }

            public async Task<PagedResponse<List<OrderResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            {

                var ordersEnumerable = _ecommerceDbContext.Orders
                    .Include(x => x.OrderItems)
                        .ThenInclude(x => x.Product)
                    .Include(x => x.Customer)
                    .Where(x => x.CustomerId == _currentUserService.UserGuid && !x.Deleted);

                if (!string.IsNullOrEmpty(request.Request.OrderByField))
                {
                    switch (request.Request.OrderByField.ToLower())
                    {
                        case "orderdate":
                            {
                                if(request.Request.OrderBy == SortBy.Ascending)
                                {
                                    ordersEnumerable = ordersEnumerable.OrderBy(x => x.OrderDate);
                                } 
                                else
                                {
                                    ordersEnumerable = ordersEnumerable.OrderByDescending(x => x.OrderDate);

                                }

                                break;
                            }
                        default: 
                            break;
                    }

                }

                var count = await ordersEnumerable.CountAsync();
                var orders = await ordersEnumerable.Skip((request.Request.PageNumber - 1) * request.Request.PageSize).Take(request.Request.PageSize).Select(x => x.ToOrderResponse()).ToListAsync();

                return new PagedResponse<List<OrderResponse>>(orders, orders.Count, request.Request.PageNumber, request.Request.PageSize);
            }
        }
    }
}
