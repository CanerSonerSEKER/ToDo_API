
using System.Net;
using System.Text.Json;
using To_Do_API.Exceptions;

namespace To_Do_API.Middleware
{
    public class ErrorHandlerMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
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

                switch (error)
                {

                    case BadRequestException:
                        // Custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest; // Gelecek olan yanıtın StatusCode'unu belirliyoruz switch case ile
                        break;


                    case NotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound; // Gelecek olan yanıtın StatusCode'unu belirliyoruz switch case ile
                        break;

                    case UnauthorizeException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError; // Gelecek olan yanıtın StatusCode'unu belirliyoruz switch case ile
                        break;
                }
                _logger.LogInformation(error.Message);
                context.Response.StatusCode = response.StatusCode;
                string resultMessage = JsonSerializer.Serialize(new { message = error?.Message });
                await context.Response.WriteAsync(resultMessage);
            }
        }
    }
}
