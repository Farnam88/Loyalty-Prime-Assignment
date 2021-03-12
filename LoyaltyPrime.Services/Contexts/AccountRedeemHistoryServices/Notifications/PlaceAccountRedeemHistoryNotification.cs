using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.Models;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountRedeemHistoryServices.Notifications
{
    public class PlaceAccountRedeemHistoryNotification : INotification
    {
        public PlaceAccountRedeemHistoryNotification()
        {
        }

        public PlaceAccountRedeemHistoryNotification(int companyRedeemId, int accountId, double redeemPoints)
        {
            CompanyRedeemId = companyRedeemId;
            AccountId = accountId;
            RedeemPoints = redeemPoints;
        }

        public int CompanyRedeemId { get; set; }
        public int AccountId { get; set; }
        public double RedeemPoints { get; set; }
    }

    public class
        PlaceAccountRedeemHistoryNotificationHandler : INotificationHandler<PlaceAccountRedeemHistoryNotification>
    {
        private readonly IUnitOfWork _uow;

        public PlaceAccountRedeemHistoryNotificationHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task Handle(PlaceAccountRedeemHistoryNotification notification,
            CancellationToken cancellationToken)
        {
            var accountRedeemHistory =
                new AccountRedeemHistory(notification.CompanyRedeemId, notification.AccountId,
                    notification.RedeemPoints);
            await _uow.AccountRedeemHistoryRepository.AddAsync(accountRedeemHistory, cancellationToken);
            await _uow.CommitAsync(cancellationToken);
        }
    }
}