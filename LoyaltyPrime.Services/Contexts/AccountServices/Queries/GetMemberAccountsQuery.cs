using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.AccountSpec;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountServices.Queries
{
    public class GetMemberAccountsQuery : IRequest<ResultModel<IList<AccountDto>>>
    {
        public GetMemberAccountsQuery(int memberId)
        {
            MemberId = memberId;
        }

        public int MemberId { get; set; }
    }

    public class
        GetMemberAccountsQueryHandler : BaseRequestHandler<GetMemberAccountsQuery, ResultModel<IList<AccountDto>>
        >
    {
        public GetMemberAccountsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResultModel<IList<AccountDto>>> Handle(GetMemberAccountsQuery request,
            CancellationToken cancellationToken)
        {
            FindMemberSpecification memberSpecification = new FindMemberSpecification(request.MemberId);
            var member = await Uow.MemberRepository.FirstOrDefaultAsync(memberSpecification, cancellationToken);
            if (member == null)
                return ResultModel<IList<AccountDto>>.NotFound(nameof(Member));

            AccountsDtoSpecification accountSpecification =
                new AccountsDtoSpecification(request.MemberId);
            var accounts = await Uow.AccountRepository.GetAllAsync(accountSpecification, cancellationToken);
            if (accounts.Count > 0)
                return ResultModel<IList<AccountDto>>.Success(200, "", accounts);
            return ResultModel<IList<AccountDto>>.Success(204);
        }
    }
}