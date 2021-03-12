using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.CompanyRewardServices.Commands
{
    public class CreateCompanyRewardCommand : IRequest<ResultModel<int>>
    {
        public CreateCompanyRewardCommand(int companyId, string rewardTitle, double rewardPoints)
        {
            CompanyId = companyId;
            RewardTitle = rewardTitle;
            RewardPoints = rewardPoints;
        }

        public int CompanyId { get; set; }
        public string RewardTitle { get; set; }
        public double RewardPoints { get; set; }
    }

    public class CreateCompanyRewardCommandHandler : BaseRequestHandler<CreateCompanyRewardCommand,
        ResultModel<int>>
    {
        public CreateCompanyRewardCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ResultModel<int>> Handle(CreateCompanyRewardCommand request,
            CancellationToken cancellationToken)
        {
            var company = await Uow.CompanyRepository.GetByIdAsync(request.CompanyId, cancellationToken);
            
            if (company == null)
                return ResultModel<int>.NotFound(nameof(Company));
            
            var companyReward =
                new CompanyReward(request.RewardTitle, request.CompanyId, request.RewardPoints);
            
            await Uow.CompanyRewardRepository.AddAsync(companyReward, cancellationToken);
            
            await Uow.CommitAsync(cancellationToken);
            
            return ResultModel<int>.Success(201, $"Company Reward {companyReward.RewardTitle} created",
                companyReward.Id);
        }
    }
}