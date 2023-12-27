using backend.Infrastructure;
using System.Net;
using System.Text.Json;

namespace backend.Middleware
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
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex).ConfigureAwait(false); ;
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case Infrastructure.ValidationException validationException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(validationException.Message);
                    break;
                case ObjectNotFoundException objectNotFound:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(objectNotFound.Message);
                    break;
                case ArgumentNullException argumentNull:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(argumentNull.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { errpr = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
