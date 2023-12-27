﻿namespace backend.Middleware
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder) 
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
