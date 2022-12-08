using Microsoft.AspNetCore.Mvc;

namespace Caching.Redis
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Cached(600)]
        public async Task<IActionResult> GetTest()
        {
            await Task.Delay(5000);
            return Ok(new { Data = "big query data" });
        }
    }
}
