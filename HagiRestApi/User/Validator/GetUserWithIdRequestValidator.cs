using FluentValidation;

namespace HagiRestApi.Controllers
{


    public class GetUserWithIdRequestValidator : AbstractValidator<GetUserWithIdRequest>
    {
        public GetUserWithIdRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .NotNull();
        }
    }
}
