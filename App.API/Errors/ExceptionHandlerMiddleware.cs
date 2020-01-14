using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace App.API.Errors
{
    public class ExceptionHandlerMiddleware
    {
        private const string JsonContentType = "application/json";
        private readonly RequestDelegate request;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            request = next;
        }

        public Task Invoke(HttpContext context) => InvokeAsync(context);

        async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await request(context);
            }
            catch (Exception exception)
            {
                var httpStatusCode = ConfigurateExceptionTypes(exception);

                context.Response.StatusCode = httpStatusCode;
                context.Response.ContentType = JsonContentType;

                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        new ErrorModel { Error = exception.Message }
                    ));

                context.Response.Headers.Clear();
            }
        }

        private static int ConfigurateExceptionTypes(Exception exception)
        {
            int httpStatusCode;

            switch (exception)
            {
                case var _ when exception is ValidationException:
                    {
                        var errorCode = (exception.Data.Contains("StatusCodeFromValidation")) ? (int)exception.Data["StatusCodeFromValidation"] : 0;
                        httpStatusCode = (errorCode != 0) ? errorCode : (int)HttpStatusCode.BadRequest;
                        break;
                    }
                default:
                    httpStatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return httpStatusCode;
        }
    }
}