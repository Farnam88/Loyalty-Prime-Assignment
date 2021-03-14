using FluentValidation;

namespace LoyaltyPrime.Services.Contexts.AccountServices.Commands
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.MemberId)
                .GreaterThan(0).WithMessage("Must be greater than 0");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Must be greater than 0");
        }
    }
}