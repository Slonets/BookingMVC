using BusinessLogic.Helpers;
using System.Net;
using System.Text.Json;

namespace New
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomHttpException httpError) //моя помилка
            {
                await CreateResponce(context, httpError.StatusCode, httpError.Message);
            }
            catch (KeyNotFoundException error)
            {
                await CreateResponce(context, HttpStatusCode.NotFound, error.Message);
            }
            catch (Exception error) //невідома помилка
            {
                await CreateResponce(context, HttpStatusCode.InternalServerError, error.Message);
            }

        }

        //Створюємо відповідь на запит

        public async Task CreateResponce(HttpContext context, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string message = "Uknow error type!")
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            //Визначення помилки
            var result = JsonSerializer.Serialize(new { message });
            await context.Response.WriteAsync(result);
        }
    }
}
