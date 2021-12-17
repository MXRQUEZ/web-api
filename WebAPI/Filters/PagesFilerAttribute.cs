using System.Linq;
using System.Net;
using Business.Exceptions;
using Business.Parameters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class PagesFilerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var value = context.ActionArguments.Values.SingleOrDefault(p => p is PageParameters);
            if (value is null)
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.NullValue);

            var pageParameters = (PageParameters) value;

            if (pageParameters.PageSize <= 0 || pageParameters.PageNumber <= 0)
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    $"{ExceptionMessage.BadParameter}s + {nameof(pageParameters.PageSize)} and" +
                    $" {nameof(pageParameters.PageNumber)} can't be less or equal 0");
        }
    }
}