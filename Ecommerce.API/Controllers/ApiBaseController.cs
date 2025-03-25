using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        protected readonly ISender _mediator;

        public ApiBaseController(ISender mediator)
        {
            _mediator = mediator;
        }
    }
}
