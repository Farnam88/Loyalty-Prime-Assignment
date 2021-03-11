using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.CompanySpec;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountServices.Commands
{
    public class CreateAccountCommand : IRequest<ResultModel<int>>
    {
        public CreateAccountCommand()
        {
        }

        public CreateAccountCommand(int companyId, int memberId)
        {
            CompanyId = companyId;
            MemberId = memberId;
        }

        public int CompanyId { get; set; }
        public int MemberId { get; set; }
    }

    public class CreateAccountCommandHandler : BaseRequestHandler<CreateAccountCommand, ResultModel<int>>
    {
        public CreateAccountCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResultModel<int>> Handle(CreateAccountCommand request,
            CancellationToken cancellationToken)
        {
            //todo: Finding existing Company should move to a MediatR query
            var companySpecification = new FindCompanySpecification(request.CompanyId);
            var company =
                await Uow.CompanyRepository.FirstOrDefaultAsync(companySpecification, cancellationToken);
            if (company == null)
                return ResultModel<int>.NotFound(nameof(Company));

            //todo: Finding existing member should move to a MediatR query
            var memberSpecification = new FindMemberSpecification(request.MemberId);
            var member = await Uow.MemberRepository.FirstOrDefaultAsync(memberSpecification, cancellationToken);
            if (member == null)
                return ResultModel<int>.NotFound(nameof(Member));

            var account = new Account(request.MemberId, request.CompanyId, 0, AccountState.Active);
            await Uow.AccountRepository.AddAsync(account, cancellationToken);
            await Uow.CommitAsync(cancellationToken);
            return ResultModel<int>.Success(201, $"Account has been created for {member.Name}", account.Id);
        }
    }
}