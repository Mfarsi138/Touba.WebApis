using Touba.WebApis.Helpers.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Touba.WebApis.Helpers.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly IHostingEnvironment _env;
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(
            IHostingEnvironment env,
            RequestDelegate next,
            ILogger logger)
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                await _next(context);
            }
            catch (Exception ex)
            {
                LogExceptionAsync(ex, context);
                await HandleExceptionAsync(ex, context);
            }
        }

        private static Task HandleExceptionAsync(Exception exception, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var newException = new Exception(exception.Message, exception.InnerException);
            newException.Data.Add("TraceIdentifier", context.TraceIdentifier);
            newException.Data.Add("StackTrace", exception.StackTrace);

            var result = JsonSerializer.Serialize(
                newException.InnerException ?? newException,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true
                });

            return context.Response.WriteAsync(result);
        }

        private async Task LogExceptionAsync(Exception exception, HttpContext context)
        {
            var msg = new StringBuilder();
            msg.AppendLine();
            msg.AppendLine("An unhandled exception occurred.");
            msg.AppendLine($"Trace Identifier: {context.TraceIdentifier}");

            if (context.Request.Path.HasValue)
                msg.AppendLine($"Request: {context.Request.Path.Value}");

            if (context.Request.QueryString.HasValue)
                msg.AppendLine($"QueryString: {context.Request.QueryString.Value}");

            if (context.Request.Method is "POST" or "PUT")
            {
                var body = await HttpHelper.GetRequestBodyAsync(context.Request);
                msg.AppendLine($"Body: {body}");
            }

            _logger.Error(exception, msg.ToString());
        }
    }
}
