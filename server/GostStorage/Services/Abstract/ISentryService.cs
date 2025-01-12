namespace GostStorage.Services.Abstract;

public interface ISentryService
{
    Task NotifyAsync(Exception exception, HttpContext httpContext);
}