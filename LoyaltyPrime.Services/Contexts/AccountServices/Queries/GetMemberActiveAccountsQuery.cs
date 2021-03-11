using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Common.Specifications.AccountSpec;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountServices.Queries
{
    public class GetMemberActiveAccountsQuery : IRequest<ResultModel>
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

    public class GetMemberActiveAccountsQueryHandler : IRequestHandler<GetMemberActiveAccountsQuery, ResultModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMemberActiveAccountsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel> Handle(GetMemberActiveAccountsQuery request, CancellationToken cancellationToken)
        {
            FindMemberSpecification memberSpecification = new FindMemberSpecification(request.MemberId);
            var member = await _unitOfWork.MemberRepository.FirstOrDefaultAsync(memberSpecification, cancellationToken);
            if (member == null)
                return ResultModel.Fail(404, "The required member does not exist!", ErrorTypes.NotFound);

            MemberActiveAccountsSpecification accountSpecification =
                new MemberActiveAccountsSpecification();
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync(accountSpecification, cancellationToken);
            if (accounts.Count > 0)
                return ResultModel.Success(200, "", accounts);
            return ResultModel.Success(204);
        }
    }
}