using Ecommerce.API.Contracts.Request.Order;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            _logger.LogDebug("Entering method {method}", nameof(CreateOrder));

            var result = await _mediator.Send(new CreateOrderCommand());

            _logger.LogDebug("Entering method {method}", nameof(CreateOrder));

            return Created(result);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditOrder(EditOrderRequest request)
        {
            _logger.LogDebug("Entering method {method}", nameof(EditOrder));

            var result = await _mediator.Send(new EditOrderCommand());

            _logger.LogDebug("Entering method {method}", nameof(EditOrder));

            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            _logger.LogDebug("Entering method {method}", nameof(DeleteOrder));

            var result = await _mediator.Send(new DeleteOrderCommand());

            _logger.LogDebug("Entering method {method}", nameof(DeleteOrder));

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            _logger.LogDebug("Entering method {method}", nameof(GetOrders));

            var result = await _mediator.Send(new GetOrdersQuery());

            _logger.LogDebug("Entering method {method}", nameof(GetOrders));

            return Ok(result);
        }
    }
}
