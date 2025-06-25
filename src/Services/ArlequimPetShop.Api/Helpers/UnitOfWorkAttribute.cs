using Microsoft.AspNetCore.Mvc.Filters;
using SrShut.Common;
using SrShut.Data;

namespace ArlequimPetShop.Api.Helpers
{
    public class UnitOfWorkAttribute : ActionFilterAttribute
    {
        private readonly IUnitOfWorkFactory _uofwFactory;

        public UnitOfWorkAttribute(IUnitOfWorkFactory uofwFactory)
        {
            Throw.ArgumentIsNull(uofwFactory, "uofwFactory");

            _uofwFactory = uofwFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Items.Add("SessionKey", _uofwFactory.Get());

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var unitOfWork = (IUnitOfWork)context.HttpContext.Items["SessionKey"];

                unitOfWork?.Dispose();
            }
            catch
            {
                //Do nothing
            }
        }
    }
}