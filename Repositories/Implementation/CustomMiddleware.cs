using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.Json;

namespace CodePulse.API.Repositories.Implementation
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandler> _logger;
        
        public CustomExceptionHandler(RequestDelegate next, ILogger<CustomExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                _logger.LogError("");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await CustomException(context, ex);
            }
        }

        private async Task CustomException(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                Message = ex.Message,
            };

            var json = JsonConvert.SerializeObject(errorResponse);
        }
    }

    //public static class CustomExceptionHandlerExtensions
    //{
    //    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    //    {
    //        app.UseMiddleware<CustomExceptionHandler>();
    //    }
    //}

    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("X-Custom-Header", "This is my custom header");
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }

    public class IPBlockingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _blockedIPs;

        public IPBlockingMiddleware(RequestDelegate next, string[] blockedIPs)
        {
            _next = next;
            _blockedIPs = blockedIPs;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

            if (_blockedIPs.Contains(remoteIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
