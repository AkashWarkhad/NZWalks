using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddelware
    {
        private readonly ILogger<ExceptionHandlerMiddelware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddelware(ILogger<ExceptionHandlerMiddelware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                // Logging id
                var errorId = Guid.NewGuid();
                logger.LogError(ex, $"{errorId}: {ex.Message}");

                // Return Custom Error Response 

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    ErrorCode = "ServerError_UnexpectedState",
                    ErrorMessage = $"Exception : {ex.Message}. We are looking into it... Thanks for ur patience. " 
                };

                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            };
        }
    }
}
