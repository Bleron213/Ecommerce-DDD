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
    public class EditOrderCommand : IRequest<OrderByIdResponse>
    {
        public EditOrderCommand(EditOrderRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            Request = request;
        }

        public EditOrderRequest Request { get; }

        public class EditOrderCommandHandler : IRequestHandler<EditOrderCommand, OrderByIdResponse>
        {
            public EditOrderCommandHandler()
            {
            }

            public Task<OrderByIdResponse> Handle(EditOrderCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
