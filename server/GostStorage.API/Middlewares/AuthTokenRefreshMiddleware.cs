namespace GostStorage.API.Middlewares
{
    public class AuthTokenRefreshMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
        }
    }
}
