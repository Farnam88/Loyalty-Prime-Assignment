using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.CompanySpec;
using LoyaltyPrime.Services.Contexts.CompanyServices.Dtos;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.CompanyServices.Queries
{
    public class GetCompaniesQuery : IRequest<ResultModel<IList<CompanyDto>>>
    {
    }

    public class GetCompaniesQueryHandler : BaseRequestHandler<GetCompaniesQuery, ResultModel<IList<CompanyDto>>>
    {
        public GetCompaniesQueryHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task<ResultModel<IList<CompanyDto>>> Handle(GetCompaniesQuery request,
            CancellationToken cancellationToken)
        {
            CompanyDtoSpecification spec = new CompanyDtoSpecification();
            var result = await Uow.CompanyRepository.GetAllAsync(spec, cancellationToken);
            if (result.Any())
                return ResultModel<IList<CompanyDto>>.Success(200, "", result);
            return ResultModel<IList<CompanyDto>>.Success(204);
        }
    }
}