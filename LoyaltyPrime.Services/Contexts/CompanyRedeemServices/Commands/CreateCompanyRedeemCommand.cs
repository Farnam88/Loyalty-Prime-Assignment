using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Commands
{
    public class CreateCompanyRedeemCommand : IRequest<ResultModel<int>>
    {
        public CreateCompanyRedeemCommand(int companyId, string redeemTitle, double redeemPoints)
        {
            CompanyId = companyId;
            RedeemTitle = redeemTitle;
            RedeemPoints = redeemPoints;
        }

        public int CompanyId { get; set; }
        public string RedeemTitle { get; set; }
        public double RedeemPoints { get; set; }
    }

    public class CreateCompanyRedeemCommandHandler : BaseRequestHandler<CreateCompanyRedeemCommand,
        ResultModel<int>>
    {
        public CreateCompanyRedeemCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResultModel<int>> Handle(CreateCompanyRedeemCommand request,
            CancellationToken cancellationToken)
        {
            var company = await Uow.CompanyRepository.GetByIdAsync(request.CompanyId, cancellationToken);

            if (company == null)
                return ResultModel<int>.NotFound(nameof(Company));

            var companyRedeem =
                new CompanyRedeem(request.RedeemTitle, request.CompanyId, request.RedeemPoints);

            await Uow.CompanyRedeemRepository.AddAsync(companyRedeem, cancellationToken);

            await Uow.CommitAsync(cancellationToken);

            return ResultModel<int>.Success(201, $"Company Reward {companyRedeem.RedeemTitle} created",
                companyRedeem.Id);
        }
    }
}