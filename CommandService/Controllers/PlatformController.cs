using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        public PlatformController()
        {

        }

        [HttpPost]
        public async Task<ActionResult> TestInboundConnection()
        {
            Console.WriteLine("--> Test successful");

            return Ok("Test okay");
        }
    }
}
