using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.CompanyRedeemSpec;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Dto;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Queries
{
    public class GetCompanyRedeemsQuery : IRequest<ResultModel<IList<CompanyRedeemDto>>>
    {
        public GetCompanyRedeemsQuery(int companyId)
        {
            CompanyId = companyId;
        }

        public int CompanyId { get; set; }
    }

    public class
        GetCompanyRedeemsQueryHandler : BaseRequestHandler<GetCompanyRedeemsQuery, ResultModel<IList<CompanyRedeemDto>>>
    {
        public GetCompanyRedeemsQueryHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task<ResultModel<IList<CompanyRedeemDto>>> Handle(GetCompanyRedeemsQuery request,
            CancellationToken cancellationToken)
        {
            var company = await Uow.CompanyRepository.GetByIdAsync(request.CompanyId, cancellationToken);
            if (company == null)
                return ResultModel<IList<CompanyRedeemDto>>.NotFound("Company");
            var specification = new CompanyRedeemsSpecification(request.CompanyId);
            var companyRewards = await Uow.CompanyRedeemRepository.GetAllAsync(specification, cancellationToken);
            if (companyRewards.Any())
                return ResultModel<IList<CompanyRedeemDto>>.Success(200, "", companyRewards);
            return ResultModel<IList<CompanyRedeemDto>>.Success(204);
        }
    }
}