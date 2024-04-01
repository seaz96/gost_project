using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace GostStorage.Services.Attributes;

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