using FluentValidation;

namespace LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Commands
{
    public class CreateCompanyRedeemCommandValidator : AbstractValidator<CreateCompanyRedeemCommand>
    {
        public CreateCompanyRedeemCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Must be greater than 0");
            
            RuleFor(x => x.RedeemTitle)
                .NotNull().NotEmpty().WithMessage("Must contain character value")
                .MaximumLength(150).WithMessage("Must not be greater than 150 characters");
            
            RuleFor(x => x.RedeemPoints)
                .GreaterThan(0).WithMessage("Must be greater than 0");
        }
    }
}