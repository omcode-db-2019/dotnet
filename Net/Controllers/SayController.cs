using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SayController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello, bitch";
        }
    }
}
