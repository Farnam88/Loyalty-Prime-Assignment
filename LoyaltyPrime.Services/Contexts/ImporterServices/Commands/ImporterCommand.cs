using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Contexts.ImporterServices.Builder;
using LoyaltyPrime.Services.Contexts.ImporterServices.Models;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.ImporterServices.Commands
{
    public class ImporterCommand : IRequest<ResultModel<Unit>>
    {
        public ImporterCommand(List<ImportModel> importObjectSet)
        {
            ImportObjectSet = importObjectSet;
        }

        public List<ImportModel> ImportObjectSet { get; set; }
    }

    public class ImporterCommandHandler : BaseRequestHandler<ImporterCommand, ResultModel<Unit>>
    {
        public ImporterCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task<ResultModel<Unit>> Handle(ImporterCommand request,
            CancellationToken cancellationToken)
        {
            var companies = new ImportModelCompanyBuilder(request.ImportObjectSet)
                .BuildCompanies()
                .GetCompanies();

            var members = request.ImportObjectSet
                .Select(s => new Member(s.Name, s.Address))
                .ToList();

            var accounts = new ImportModelAccountBuilder(companies, members, request.ImportObjectSet)
                .BuildAccounts()
                .GetAccounts();

            await Uow.CompanyRepository.AddRangeAsync(companies, cancellationToken);
            await Uow.MemberRepository.AddRangeAsync(members, cancellationToken);
            await Uow.AccountRepository.AddRangeAsync(accounts, cancellationToken);
            
            await Uow.CommitAsync(cancellationToken);

            return ResultModel<Unit>.Success(200, "Import completed", Unit.Value);
        }
    }
}