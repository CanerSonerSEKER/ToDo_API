using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using To_Do_API.Exceptions;

namespace To_Do_API.Middleware
{
    public class ErrorHandlerMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, RequestDelegate next, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Context mevcut istegi belirtiyor.
            }
            catch (Exception error)
            {
                HttpResponse response = context.Response; // Mecvut istegin response'unu ifade ediyor.
                response.ContentType = "application/json"; // yanıtın geri dönüş tipini belirtiyor.
                string? clientMessage;
                string title;
                string errorCode;

                switch (error)
                {

                    case BadRequestException:
                        // Custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest; // Gelecek olan yanıtın StatusCode'unu belirliyoruz switch case ile
                        title = "Bad Request";
                        errorCode = "BAD_REQUEST";                        
                        _logger.LogWarning("Hata oluştu. {Message}", error.Message);
                        clientMessage = _hostEnvironment.IsDevelopment() 
                            ? error.Message 
                            : "The request could not be processed. Please check your input and try again.";
                        break;


                    case NotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound; // Gelecek olan yanıtın StatusCode'unu belirliyoruz switch case ile
                        title = "Not Found";
                        errorCode = "NOT_FOUND";
                        _logger.LogWarning("Hata oluştu. {Message}", error.Message);
                        clientMessage = _hostEnvironment.IsDevelopment() 
                            ? error.Message 
                            : "The requested resource was not found.";
                        break;

                    case UnauthorizeException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        title = "Unauthorized";
                        errorCode = "UNAUTHORIZED";
                        _logger.LogWarning("Hata oluştu. {Message}", error.Message);
                        clientMessage = _hostEnvironment.IsDevelopment() 
                            ? error.Message 
                            : "Authentication failed. Please log in again.";
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError; // Gelecek olan yanıtın StatusCode'unu belirliyoruz switch case ile
                        title = "Internal Server Error";
                        errorCode = "INTERNAL_ERROR";
                        _logger.LogError("Hata oluştu. {Message}", error.Message);
                        clientMessage = _hostEnvironment.IsDevelopment() 
                            ? error.Message 
                            : "An unexpected error occurred. Please contact support with the trace ID.";
                        break;
                }

                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Status = response.StatusCode, 
                    Title = title,
                    Detail = clientMessage, 
                    Type = "about:blank"
                };
                problemDetails.Extensions["traceId"] = context.TraceIdentifier;
                problemDetails.Extensions["errorCode"] = errorCode;

                string resultMessage = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(resultMessage);   
            }
        }
    }
}
