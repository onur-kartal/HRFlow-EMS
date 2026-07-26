using System.Diagnostics;
using System.Security.Claims;
using HRFlow.Business.DTOs.Logging;
using HRFlow.Business.Interfaces;

namespace HRFlow.Web.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public RequestLoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsStaticRequest(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            try { await _next(context); }
            finally
            {
                stopwatch.Stop();
                await WriteLogAsync(context, stopwatch.ElapsedMilliseconds);
            }
        }

        private async Task WriteLogAsync(HttpContext context, long durationMs)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IRequestLogService>();
                var userAgent = context.Request.Headers.UserAgent.ToString();
                await service.LogAsync(new RequestLogCreateDto
                {
                    UserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    UserName = context.User.Identity?.Name,
                    Role = context.User.FindFirstValue(ClaimTypes.Role),
                    IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                    RequestPath = context.Request.Path,
                    HttpMethod = context.Request.Method,
                    StatusCode = context.Response.StatusCode,
                    DurationMs = durationMs,
                    UserAgent = userAgent,
                    Browser = GetBrowser(userAgent),
                    OperatingSystem = GetOperatingSystem(userAgent)
                });
            }
            catch { }
        }

        private static bool IsStaticRequest(PathString path) =>
            path.StartsWithSegments("/css") || path.StartsWithSegments("/js") ||
            path.StartsWithSegments("/images") || path.StartsWithSegments("/adminlte") ||
            path.StartsWithSegments("/plugins") ||
            path.Value?.Equals("/favicon.ico", StringComparison.OrdinalIgnoreCase) == true || Path.HasExtension(path);

        private static string GetBrowser(string userAgent) =>
            userAgent.Contains("Edg/", StringComparison.OrdinalIgnoreCase) ? "Edge" :
            userAgent.Contains("Chrome/", StringComparison.OrdinalIgnoreCase) ? "Chrome" :
            userAgent.Contains("Firefox/", StringComparison.OrdinalIgnoreCase) ? "Firefox" :
            userAgent.Contains("Safari/", StringComparison.OrdinalIgnoreCase) ? "Safari" : "Unknown";

        private static string GetOperatingSystem(string userAgent) =>
            userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase) ? "Windows" :
            userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase) ? "Android" :
            userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase) || userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase) ? "iOS" :
            userAgent.Contains("Mac OS", StringComparison.OrdinalIgnoreCase) ? "macOS" :
            userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase) ? "Linux" : "Unknown";
    }
}
