using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;

namespace HagiRestApi
{

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationContext = new ValidationContext<TRequest>(request);

            var validationFailures = await GetValidationFailures(validationContext);

            var hasAnyValidationFailed = validationFailures.Any();

            if (hasAnyValidationFailed)
            {
                throw new ValidationException(validationFailures);
            }


            var response = await next();
            return response;
        }

        private async Task<List<ValidationFailure>> GetValidationFailures(ValidationContext<TRequest> validationContext)
        {
            var validationFailures = await Task.WhenAll(
                _validators.Select(validator =>
                {
                    return validator.ValidateAsync(validationContext);
                }));


            return validationFailures
                .Where(validationResult => validationResult.IsValid == false)
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure => new ValidationFailure()
                {
                    PropertyName = validationFailure.PropertyName,
                    ErrorMessage = validationFailure.ErrorMessage,
                })
                .ToList();
        }
    }
}
