using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using TaskPlanner.BLL.Exceptions;

namespace TaskPlanner.PL.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = exception switch
                {
                    ProjectAlreadyTakenException => 400,
                    ProjectNotTakenException => 400,
                    TaskAlreadyTakenException => 400,
                    TaskNotTakenException => 400,
                    UserAccesDeniedException => 400,
                    UserNameDuplicateException => 400,
                    NullReferenceException => 400,
                    KeyNotFoundException => 404,
                    _ => 500
                };

                var result = JsonSerializer.Serialize(new { message = exception?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
