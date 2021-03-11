using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.MemberServices.Queris
{
    public class GetMembersQuery : IRequest<ResultModel<IList<MemberDto>>>
    {
    }

    public class GetMembersQueryHandler : BaseRequestHandler<GetMembersQuery, ResultModel<IList<MemberDto>>>
    {
        public GetMembersQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResultModel<IList<MemberDto>>> Handle(GetMembersQuery request,
            CancellationToken cancellationToken)
        {
            GetMembersDtoSpecification specification = new GetMembersDtoSpecification();

            var result = await Uow.MemberRepository.GetAllAsync(specification, cancellationToken);
            return ResultModel<IList<MemberDto>>.Success(200, string.Empty, result);
        }
    }
}