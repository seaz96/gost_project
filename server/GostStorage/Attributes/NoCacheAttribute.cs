using Microsoft.AspNetCore.Mvc.Filters;

namespace GostStorage.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class NoCacheAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        filterContext.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        filterContext.HttpContext.Response.Headers["Expires"] = "-1";
        filterContext.HttpContext.Response.Headers["Pragma"] = "no-cache";

        base.OnResultExecuting(filterContext);
    }
}