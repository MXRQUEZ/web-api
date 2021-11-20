using System.Linq;
using System.Net;
using Business.Exceptions;
using Business.Parameters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class PagesValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var (_, value) = context.ActionArguments.SingleOrDefault(p => p.Value is PageParameters);
            var pageParameters = (PageParameters)value;

            if (pageParameters.PageSize <= 0 || pageParameters.PageNumber < 0)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.BadParameter);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}
