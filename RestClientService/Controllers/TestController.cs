using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestClientService.Service;

namespace RestClientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IRestcs restcs;
        public TestController(IRestcs _restcs)
        {
            restcs = _restcs;
        }

        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> TestValues()
        {
           var userlist =  await restcs.makegetCall();
            return Ok(userlist);
        }
    }
}
