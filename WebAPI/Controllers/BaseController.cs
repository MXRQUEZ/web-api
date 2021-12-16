using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger Logger;

        protected BaseController(ILogger logger) => Logger = logger;
    }
}
