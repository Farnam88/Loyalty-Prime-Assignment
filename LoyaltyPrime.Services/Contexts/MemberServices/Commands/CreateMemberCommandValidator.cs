using FluentValidation;
using LoyaltyPrime.Services.Contexts.ImporterServices.Commands;

namespace LoyaltyPrime.Services.Contexts.MemberServices.Commands
{
    public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
    {
        public CreateMemberCommandValidator()
        {
            RuleFor(x => new
                {
                    x.Name,
                    x.Address
                })
                .NotNull().NotEmpty().WithMessage("Must contain character value");

            RuleFor(x => x.Name)
                .MaximumLength(150).WithMessage("Must not be greater than 150 characters");

            RuleFor(x => x.Name)
                .MaximumLength(500).WithMessage("Must not be greater than 500 characters");
        }
    }
}