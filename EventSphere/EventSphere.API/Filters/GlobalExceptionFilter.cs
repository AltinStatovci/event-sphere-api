using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is EventSphere.API.Filters.ValidationException validationException)
            {
                context.Result = new BadRequestObjectResult(validationException.Errors);
                context.ExceptionHandled = true;
            }
        }

    }
}
