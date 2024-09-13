using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<string> Get()
        {
            return Ok("Hi!");
        }
    }
}