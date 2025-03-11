using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Events.MiddleWares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await WriteError(httpContext, ex.Errors);
            }
            catch (InvalidOperationException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await WriteError(httpContext, ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await WriteError(httpContext, ex.Message);
            }
            catch (SecurityTokenException ex)
            {
                httpContext.Response.StatusCode= (int)HttpStatusCode.Unauthorized;
                await WriteError(httpContext, ex.Message);
            }
            catch (ArgumentException ex)
            {
                httpContext.Response.StatusCode=(int)HttpStatusCode.Unauthorized;
                await WriteError(httpContext, ex.Message);
            }
            catch (AggregateException ex)
            {
                httpContext.Response.StatusCode= (int)HttpStatusCode.UnsupportedMediaType;
                await WriteError(httpContext, ex.Message);
            }
        }

        private static Task WriteError(HttpContext httpContext, IEnumerable<FluentValidation.Results.ValidationFailure> errors)
        {
            var errorResponse = new
            {
                Errors = errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                StatusCode = httpContext.Response.StatusCode,
                Timestamp = DateTime.UtcNow
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync(jsonResponse);
        }

        private static Task WriteError(HttpContext httpContext, string message)
        {
            var errorResponse = new
            {
                Errors = new List<FluentValidation.Results.ValidationFailure>() { new("error", message) },
                StatusCode = httpContext.Response.StatusCode,
                Timestamp = DateTime.UtcNow
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync(jsonResponse);
        }
    }
}
