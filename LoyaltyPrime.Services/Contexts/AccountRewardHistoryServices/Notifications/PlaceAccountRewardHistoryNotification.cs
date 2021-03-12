using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountRewardHistoryServices.Notifications
{
    public class PlaceAccountRewardHistoryNotification : INotification
    {
        public PlaceAccountRewardHistoryNotification(int companyRewardId, int accountId, double rewardPoints)
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
        PlaceAccountRewardHistoryNotificationHandler : INotificationHandler<PlaceAccountRewardHistoryNotification>
    {
        private readonly IUnitOfWork _uow;

        public PlaceAccountRewardHistoryNotificationHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(PlaceAccountRewardHistoryNotification notification, CancellationToken cancellationToken)
        {
            var accountRewardHistory =
                new AccountRewardHistory(notification.CompanyRewardId, notification.AccountId,
                    notification.RewardPoints);
            
            await _uow.AccountRewardHistoryRepository.AddAsync(accountRewardHistory, cancellationToken);
            
            await _uow.CommitAsync(cancellationToken);
        }
    }
}