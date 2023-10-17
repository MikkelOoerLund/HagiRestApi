using FluentValidation;

namespace HagiRestApi.Controllers
{
    public class GetUserWithIdRequestValidator : AbstractValidator<GetUserWithIdRequest>
    {
        public GetUserWithIdRequestValidator()
        {
            RuleFor(x => x.UserId)
                .LessThanOrEqualTo(0)
                .NotNull();
        }
    }
}
