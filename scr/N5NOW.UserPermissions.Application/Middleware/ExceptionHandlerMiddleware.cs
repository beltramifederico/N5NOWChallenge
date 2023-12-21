using Microsoft.AspNetCore.Http;
using N5NOW.UserPermissions.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace N5NOW.UserPermissions.Application.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context, error.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode httpResponse, IEnumerable<string>? errors = null)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var result = new ExceptionResponse((int)httpResponse, message, errors);
            response.StatusCode = (int)httpResponse;
            await response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
