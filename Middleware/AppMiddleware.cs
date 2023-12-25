using System.Net;

namespace OrganizationApi.Middleware;

public class AppMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            // Make the error the same throughout the app.
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
            await context.Response.CompleteAsync();
        }
    }
}