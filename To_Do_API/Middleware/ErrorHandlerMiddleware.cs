
using System.Net;
using System.Text.Json;
using To_Do_API.Exceptions;

namespace To_Do_API.Middleware
{
    public class ErrorHandlerMiddleware : IMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        //BURADA KALDIM 
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context); // Context mevcut istegi belirtiyor.
            }
            catch (Exception error)
            {
                HttpResponse response = context.Response; // Mecvut istegin response'unu ifade ediyor.
                response.ContentType = "application/json"; // yanıtın geri dönüş tipini belirtiyor.

                switch (error)
                {

                    case BadRequestException:
                        // Custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;


                    case NotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                _logger.LogInformation(error.Message);
                context.Response.StatusCode = response.StatusCode;
                var resultMessage = JsonSerializer.Serialize(new { message = error?.Message });
                await context.Response.WriteAsync(resultMessage);
            }
        }
    }
}
