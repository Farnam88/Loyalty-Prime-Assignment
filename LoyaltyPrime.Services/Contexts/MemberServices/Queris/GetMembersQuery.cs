using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.MemberServices.Queris
{
    public class GetMembersQuery : IRequest<ResultModel>
    {
    }

    public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, ResultModel>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMembersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultModel> Handle(GetMembersQuery request, CancellationToken cancellationToken)
        {
            GetMembersDtoSpecification specification = new GetMembersDtoSpecification();
            
            var result = await _unitOfWork.MemberRepository.GetAllAsync(specification, cancellationToken);
            return ResultModel.Success(200, string.Empty, result);
        }
    }
}