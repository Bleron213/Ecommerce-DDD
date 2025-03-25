using Ecommerce.API.Contracts.Request.Order;
using Ecommerce.API.Contracts.Response.Order;
using MediatR;
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
            public CreateOrderCommandHandler()
            {
            }

            public Task<OrderByIdResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
