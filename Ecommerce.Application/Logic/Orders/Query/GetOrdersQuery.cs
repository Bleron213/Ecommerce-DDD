using Ecommerce.API.Contracts.Response.Order;
using MediatR;
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
            public GetOrdersQueryHandler()
            {
            }

            public Task<List<OrderResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
