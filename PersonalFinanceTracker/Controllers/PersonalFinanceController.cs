using Microsoft.AspNetCore.Mvc;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/[controller]")]

    public class FinanceController : ControllerBase
    {
        [HttpGet("endpoint1")]
        public IActionResult GetEndpoint1()
        {
            return Ok("This is the first endpoint");
        }

        [HttpGet("endpoint2")]
        public IActionResult GetEndpoint2()
        {
            return Ok("This is the second endpoint");
        }
    }
}
