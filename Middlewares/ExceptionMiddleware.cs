using IMDBSample.Exceptions;
using IMDBSample.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace IMDBSample.Middlewares
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
                await _next(context); // go to next middleware / controller
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var apiResponse = new ApiResponse<object>
            {
                Success = false
            };

            switch (ex)
            {
                case InvalidRequestDataException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    apiResponse.Message = validationEx.Message;
                    break;

                case EntityNotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    apiResponse.Message = notFoundEx.Message;
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    apiResponse.Message = unauthorizedEx.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    apiResponse.Message = ex.Message; //clear message 
                    break;
            }

            var json = JsonSerializer.Serialize(apiResponse);
            return response.WriteAsync(json);
        }
    }
}
