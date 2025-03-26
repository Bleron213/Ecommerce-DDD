using Ecommerce.API.Common.Exceptions;
using Ecommerce.API.Contracts.Mapping;
using Ecommerce.API.Contracts.Response.Order;
using Ecommerce.Application.Abstractions.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Logic.Orders.Query
{
    public class GetOrdersQuery : IRequest<List<OrderResponse>>
    {
        public GetOrdersQuery()
        {
        }

        public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderResponse>>
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

            public async Task<List<OrderResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            {
                var orders = await _ecommerceDbContext.Orders
                        .Include(x => x.OrderItems)
                            .ThenInclude(x => x.Product)
                        .Include(x => x.Customer)
                        .Where(x => x.CustomerId == _currentUserService.UserGuid && !x.Deleted)
                        .ToListAsync();


                return orders.Select(x => x.ToOrderResponse()).ToList();
            }
        }
    }
}
