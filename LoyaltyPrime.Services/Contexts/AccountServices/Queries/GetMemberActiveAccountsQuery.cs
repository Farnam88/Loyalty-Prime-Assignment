using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.AccountSpec;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountServices.Queries
{
    public class GetMemberActiveAccountsQuery : IRequest<ResultModel<object>>
    {
        public GetMemberActiveAccountsQuery()
        {
        }

        public GetMemberActiveAccountsQuery(int memberId)
        {
            MemberId = memberId;
        }

        public int MemberId { get; set; }
    }

    public class
        GetMemberActiveAccountsQueryHandler : BaseRequestHandler<GetMemberActiveAccountsQuery, ResultModel<object>>
    {
        public GetMemberActiveAccountsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResultModel<object>> Handle(GetMemberActiveAccountsQuery request,
            CancellationToken cancellationToken)
        {
            FindMemberSpecification memberSpecification = new FindMemberSpecification(request.MemberId);
            var member = await Uow.MemberRepository.FirstOrDefaultAsync(memberSpecification, cancellationToken);
            if (member == null)
                return ResultModel<object>.NotFound(nameof(Member));

            AccountsDtoSpecification accountSpecification =
                new AccountsDtoSpecification();
            var accounts = await Uow.AccountRepository.GetAllAsync(accountSpecification, cancellationToken);
            if (accounts.Count > 0)
                return ResultModel<object>.Success(200, "", accounts);
            return ResultModel<object>.Success(204);
        }
    }
}