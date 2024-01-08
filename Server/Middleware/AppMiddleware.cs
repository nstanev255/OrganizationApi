using System.Net;

namespace OrganizationApi.Middleware;

public class AppMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            context.Response.Headers["Custom-Header"] = "Custom header";

            
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            if (ipAddress != "127.0.0.1" && ipAddress != "::1")
            {
                throw new Exception("The request comes from an unknown source.");
            }

            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            Console.WriteLine("exception occured " + exception.Message);
            // Make the error the same throughout the app.
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
            await context.Response.CompleteAsync();
        }
    }
}