using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace eCommerce.SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string message = "Sorry, Internal server error occurred, kindly try again";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string tittle = "error";
            try
            {
                await next(context);
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    tittle = "Warning";
                    message = "Too many requests made";
                    statusCode = StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, tittle, message, statusCode);
                }

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    tittle = "Alert";
                    message = "You are not authorized to access";
                    statusCode = StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, tittle, message, statusCode);
                }

                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    tittle = "No Access";
                    message = "You are not allowed to access";
                    statusCode = StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, tittle, message, statusCode);
                }

            }
            catch (Exception ex)
            {
                //LogException.LogExceptions(ex);

                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    tittle = "out of time";
                    message = "Request timeout... try agin";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }

                await ModifyHeader(context, tittle, message, statusCode);
            }
        }

        private async Task ModifyHeader(HttpContext context, string tittle, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = tittle
            }), CancellationToken.None);
            return;
        }
    }
}
