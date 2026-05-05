using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeFlow.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problem = exception switch
            {
                UnauthorizedAccessException => CreateProblem(
                    status: HttpStatusCode.Unauthorized,
                    title: "Unauthorized",
                    detail: exception.Message),
                    
                KeyNotFoundException => CreateProblem(
                    status: HttpStatusCode.NotFound,
                    title: "Resource not found",
                    detail: exception.Message),

                ArgumentException => CreateProblem(
                    status: HttpStatusCode.BadRequest,
                    title: "Invalid request",
                    detail: exception.Message),

                _ => CreateProblem(
                    status: HttpStatusCode.InternalServerError,
                    title: "Internal server error",
                    detail: "An unexpected error occurred.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = problem.Status ?? 500;

            var json = JsonSerializer.Serialize(problem);

            await context.Response.WriteAsync(json);
        }

        private static ProblemDetails CreateProblem(HttpStatusCode status, string title, string detail)
        {
            return new ProblemDetails
            {
                Status = (int)status,
                Title = title,
                Detail = detail,
                Type = $"https://httpstatuses.com/{(int)status}"
            };
        }
    }
}