using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SrShut.Common.Exceptions;

namespace ArlequimPetShop.Api.Controllers
{
    public class BaseController : Controller
    {
        public BaseController() { }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (context.Exception != null)
            {
                if (context.Exception is HttpException)
                {
                    var exception = context.Exception as HttpException;

                    if (exception.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        context.ExceptionHandled = true;
                        context.Result = Forbid();
                    }
                }
            }
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await base.OnActionExecutionAsync(context, next);
        }
    }
}