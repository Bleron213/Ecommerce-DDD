using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Get()
        {
            return Ok("Pong");
        }
    }
}
