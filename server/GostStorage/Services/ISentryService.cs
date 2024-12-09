namespace GostStorage.Services;

public interface ISentryService
{
    Task NotifyAsync(Exception exception, HttpContext httpContext);
}