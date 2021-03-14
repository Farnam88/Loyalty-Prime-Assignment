using System.Linq;
using FluentValidation;
using LoyaltyPrime.Services.Contexts.ImporterServices.Models;

namespace LoyaltyPrime.Services.Contexts.ImporterServices.Commands
{
    public class ImporterCommandValidator : AbstractValidator<ImporterCommand>
    {
        public ImporterCommandValidator()
        {
            RuleFor(x => x.ImportObjectSet)
                .NotNull().NotEmpty().WithMessage("Import Json Must contain member with account details");

            RuleForEach(f => f.ImportObjectSet)
                .SetValidator(new ImportModelValidator());
        }
    }

    public class ImportModelValidator : AbstractValidator<ImportModel>
    {
        public ImportModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty().WithMessage("Must contain character value");

            RuleFor(x => x.Address)
                .NotNull().NotEmpty().WithMessage("Must contain character value");

            RuleFor(x => x.Name)
                .MaximumLength(150).WithMessage("Must not be greater than 150 characters");

            RuleFor(x => x.Name)
                .MaximumLength(500).WithMessage("Must not be greater than 500 characters");
            RuleForEach(f => f.Accounts)
                .Where(x=>x!=null)
                .SetValidator(new ImportAccountModelValidator());
        }
    }

    public class ImportAccountModelValidator : AbstractValidator<ImportAccountModel>
    {
        public ImportAccountModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty().WithMessage("Must contain character value");

            RuleFor(x => x.Status)
                .NotNull().NotEmpty().WithMessage("Must contain character value");

            RuleFor(x => x.Name)
                .MaximumLength(150).WithMessage("Must not be greater than 150 characters");
        }
    }
}