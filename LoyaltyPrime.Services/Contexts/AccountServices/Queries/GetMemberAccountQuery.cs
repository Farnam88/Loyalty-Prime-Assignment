using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.AccountSpec;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountServices.Queries
{
    public class GetMemberAccountQuery : IRequest<ResultModel<AccountDto>>
    {
        public GetMemberAccountQuery(int memberId, int accountId)
        {
            AccountId = accountId;
            MemberId = memberId;
        }

        public int AccountId { get; set; }
        public int MemberId { get; set; }
    }

    public class GetAccountByIdQueryHandler : BaseRequestHandler<GetMemberAccountQuery, ResultModel<AccountDto>>
    {
        public GetAccountByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResultModel<AccountDto>> Handle(GetMemberAccountQuery request,
            CancellationToken cancellationToken)
        {
            AccountsDtoSpecification specification =
                new AccountsDtoSpecification(request.MemberId, request.AccountId);
            var account = await Uow.AccountRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (account != null)
                return ResultModel<AccountDto>.Success(200, "", account);
            return ResultModel<AccountDto>.Fail(404, "Requested Account not found",ErrorTypes.LogicalError);
        }
    }
}