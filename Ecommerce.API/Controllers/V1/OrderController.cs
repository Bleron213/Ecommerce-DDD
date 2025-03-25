using Ecommerce.API.Contracts.Request.Order;
using Ecommerce.API.Contracts.Response.Order;
using Ecommerce.Application.Logic.Orders.Commands;
using Ecommerce.Application.Logic.Orders.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Ecommerce.API.Controllers.V1
{
    public class OrderController : ApiBaseController
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            ISender mediator,
            ILogger<OrderController> logger
            ) : base(mediator)
        {
            _logger = logger;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(OrderByIdResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Description("Creates a new order in the system.")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            _logger.LogDebug("Entering method {method}", nameof(CreateOrder));

            var result = await _mediator.Send(new CreateOrderCommand(request));

            _logger.LogDebug("Entering method {method}", nameof(CreateOrder));

            return Ok(result);
        }

        [HttpPost("edit")]
        [ProducesResponseType(typeof(OrderByIdResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Description("Edits an existing order")]
        public async Task<IActionResult> EditOrder(EditOrderRequest request)
        {
            _logger.LogDebug("Entering method {method}", nameof(EditOrder));

            var result = await _mediator.Send(new EditOrderCommand(request));

            _logger.LogDebug("Entering method {method}", nameof(EditOrder));

            return Ok(result);
        }

        [HttpPost("delete")]
        [ProducesResponseType(typeof(OrderByIdResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Description("Deletes an order")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            _logger.LogDebug("Entering method {method}", nameof(DeleteOrder));

            await _mediator.Send(new DeleteOrderCommand(orderId));

            _logger.LogDebug("Entering method {method}", nameof(DeleteOrder));

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrderResponse>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Description("Retrieves orders")]
        public async Task<IActionResult> GetOrders()
        {
            _logger.LogDebug("Entering method {method}", nameof(GetOrders));

            var result = await _mediator.Send(new GetOrdersQuery());

            _logger.LogDebug("Entering method {method}", nameof(GetOrders));

            return Ok(result);
        }
    }
}
