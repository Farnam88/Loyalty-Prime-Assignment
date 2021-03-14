using FluentValidation;

namespace LoyaltyPrime.Services.Contexts.CompanyRewardServices.Commands
{
    public class CreateCompanyRewardCommandValidator : AbstractValidator<CreateCompanyRewardCommand>
    {
        public CreateCompanyRewardCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Must be greater than 0");

            RuleFor(x => x.RewardTitle)
                .NotNull().NotEmpty().WithMessage("Must contain character value")
                .MaximumLength(150).WithMessage("Must not be greater than 150 characters");

            RuleFor(x => x.RewardPoints)
                .GreaterThan(0).WithMessage("Must be greater than 0");
        }
    }
}