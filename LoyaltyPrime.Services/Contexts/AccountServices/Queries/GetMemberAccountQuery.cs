using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.AccountSpec;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountServices.Queries
{
    public class GetMemberAccountQuery : IRequest<ResultModel<MemberAccountsDto>>
    {
        public GetMemberAccountQuery(int accountId, int memberId)
        {
            AccountId = accountId;
            MemberId = memberId;
        }

        public int AccountId { get; set; }
        public int MemberId { get; set; }
    }

    public class GetAccountByIdQueryHandler : BaseRequestHandler<GetMemberAccountQuery, ResultModel<MemberAccountsDto>>
    {
        public GetAccountByIdQueryHandler(IUnitOfWork unitOfWork):base(unitOfWork)
        {
        }

        public override async Task<ResultModel<MemberAccountsDto>> Handle(GetMemberAccountQuery request,
            CancellationToken cancellationToken)
        {
            MemberAccountsSpecification specification =
                new MemberAccountsSpecification(request.AccountId, request.MemberId);
            var account = await Uow.AccountRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (account != null)
                return ResultModel<MemberAccountsDto>.Success(200, "", account);
            return ResultModel<MemberAccountsDto>.Fail(400, "Requested Account not found");
        }
    }
}