using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.CompanyRewardSpec;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Dto;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.CompanyRewardServices.Queries
{
    public class GetCompanyRewardsQuery : IRequest<ResultModel<IList<CompanyRewardDto>>>
    {
        public GetCompanyRewardsQuery(int companyId)
        {
            CompanyId = companyId;
        }

        public int CompanyId { get; set; }
    }

    public class
        GetCompanyRewardsQueryHandler : BaseRequestHandler<GetCompanyRewardsQuery, ResultModel<IList<CompanyRewardDto>>>
    {
        public GetCompanyRewardsQueryHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task<ResultModel<IList<CompanyRewardDto>>> Handle(GetCompanyRewardsQuery request,
            CancellationToken cancellationToken)
        {
            var company = await Uow.CompanyRepository.GetByIdAsync(request.CompanyId, cancellationToken);
            if (company == null)
                return ResultModel<IList<CompanyRewardDto>>.NotFound("Company");
            var specification = new CompanyRewardsSpecification(request.CompanyId);
            var companyRewards = await Uow.CompanyRewardRepository.GetAllAsync(specification, cancellationToken);
            if (companyRewards.Any())
                return ResultModel<IList<CompanyRewardDto>>.Success(200, "", companyRewards);
            return ResultModel<IList<CompanyRewardDto>>.Success(204);
        }
    }
}