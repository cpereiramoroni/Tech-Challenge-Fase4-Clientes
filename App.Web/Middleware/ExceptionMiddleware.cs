using App.Application.CustomExceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                await HandleClientExceptionAsync(context, ex);
            }
            catch (ValidationException ex)
            {
                await HandleClientExceptionAsync(context, ex);
            }
            catch (ClientException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleClientExceptionAsync(
         HttpContext context, ClientException Clientexception)
        {
            var response = new
            {
                message = Clientexception.Message,
                erros = Clientexception.Errors
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static Task HandleExceptionAsync(
            HttpContext context, Exception exception)
        {
            var response = new { message = exception.Message };
            return Response(context, response);
        }

        private static Task HandleClientExceptionAsync(
            HttpContext context, Exception Clientexception)
        {
            var response = new
            {
                message = Clientexception.Message,
            };
            return Response(context, response);
        }

        private static Task Response(HttpContext context, object response)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

}
