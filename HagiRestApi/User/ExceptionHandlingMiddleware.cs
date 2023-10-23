using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace HagiRestApi
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                var problemDetails = GetProblemDetails(exception);

                var response = httpContext.Response;

                var statusCode = (int)problemDetails.Status;
                response.StatusCode = statusCode;

                await response.WriteAsJsonAsync(problemDetails);
            }
        }

        private ProblemDetails GetProblemDetails(Exception exception)
        {
            switch (exception)
            {
                case ValidationException validationException: return CreateProblemDetailsFrom(validationException);

                default: throw exception;
            }
        }


        private ProblemDetails CreateProblemDetailsFrom(ValidationException validationException)
        {

            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Type =  "ValidationFailure",
                Title = "ValidationError",
                Detail = "One or more validation errors has occured",
            };

            var errors = validationException.Errors;
            problemDetails.Extensions["errors"] = errors;
            return problemDetails;
        }




    }
}
