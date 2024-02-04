using ChitChat.Core.Errors;
using Serilog;
using System.Net;

namespace ChitChat.Web.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            if (context != null)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;
                Log.Error("An error occurred while processing your request");
                await context.Response.WriteAsJsonAsync(new Error
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error"
                });
            }
        }
    }
}
