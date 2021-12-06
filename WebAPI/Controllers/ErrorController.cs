using System.Net;
using Business.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [Route("/error")]
        public ActionResult<ErrorResponse> Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var code = (int)HttpStatusCode.InternalServerError;

            if (exception is HttpStatusException httpStatusException)
            {
                code = (int) httpStatusException.Status;
                Response.StatusCode = code;
                return new ErrorResponse(exception);
            }

            Response.StatusCode = code;

            return View("~/Views/Error.cshtml");
        }
    }
}
