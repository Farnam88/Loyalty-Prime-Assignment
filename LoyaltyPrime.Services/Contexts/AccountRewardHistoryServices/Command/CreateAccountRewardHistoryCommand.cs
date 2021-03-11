using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountRewardHistoryServices.Command
{
    public class CreateAccountRewardHistoryCommand : IRequest<ResultModel<int>>
    {
        public CreateAccountRewardHistoryCommand()
        {
        }

        public CreateAccountRewardHistoryCommand(int companyRewardId, int accountId, double rewardPoints)
        {
            CompanyRewardId = companyRewardId;
            AccountId = accountId;
            RewardPoints = rewardPoints;
        }

        public int CompanyRewardId { get; set; }
        public int AccountId { get; set; }
        public double RewardPoints { get; set; }
    }

    public class
        CreateAccountRewardHistoryCommandHandler : BaseRequestHandler<CreateAccountRewardHistoryCommand,
            ResultModel<int>>
    {
        public CreateAccountRewardHistoryCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task<ResultModel<int>> Handle(CreateAccountRewardHistoryCommand request,
            CancellationToken cancellationToken)
        {
            var accountRewardHistory =
                new AccountRewardHistory(request.CompanyRewardId, request.AccountId, request.RewardPoints);
            await Uow.AccountRewardHistoryRepository.AddAsync(accountRewardHistory, cancellationToken);
            await Uow.CommitAsync(cancellationToken);
            return ResultModel<int>.Success(201, "Account reward history created", accountRewardHistory.Id);
        }
    }
}