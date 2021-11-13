using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger Logger;

        public BaseController(ILogger logger)
        {
            Logger = logger;
        }
    }
}
