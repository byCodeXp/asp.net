using System;
using System.Net;
using System.Threading.Tasks;
using Api.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models.Responses;

namespace Api.Middlewares
{
    public class ExceptionsHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionsHandlerMiddleware> _logger;

        public ExceptionsHandlerMiddleware(RequestDelegate next, ILogger<ExceptionsHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                HttpResponse response = context.Response;
                response.ContentType = "application/json";

                string message = null;
                object errors = null;

                switch (exception)
                {
                    case NotFoundRestException ex:
                        response.StatusCode = (int) HttpStatusCode.NotFound;
                        message = ex.Message;
                        errors = ex.Errors;
                        _logger.LogError(ex, message);
                        break;
                    case BadRequestRestException ex:
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        message = ex.Message;
                        errors = ex.Errors;
                        _logger.LogError(ex, message);
                        break;
                    default:
                        response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        _logger.LogError(exception, "Internal Server Error");
                        break;
                }

                await response.WriteAsync(new FailedResponse
                {
                    StatusCode = response.StatusCode,
                    Message = message,
                    Errors = errors
                }.ToString());
            }
        }
    }
}