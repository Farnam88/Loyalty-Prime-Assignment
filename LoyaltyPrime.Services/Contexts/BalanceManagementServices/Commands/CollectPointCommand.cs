using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.AccountSpec;
using LoyaltyPrime.Services.Contexts.AccountRewardHistoryServices.Notifications;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.BalanceManagementServices.Commands
{
    public class CollectPointCommand : IRequest<ResultModel<double>>
    {
        public CollectPointCommand()
        {
        }

        public CollectPointCommand(int accountId, int memberId, int companyRewardId)
        {
            AccountId = accountId;
            MemberId = memberId;
            CompanyRewardId = companyRewardId;
        }

        public int AccountId { get; set; }
        public int MemberId { get; set; }
        public int CompanyRewardId { get; set; }
    }

    public class CollectPointCommandHandler : BaseRequestHandler<CollectPointCommand, ResultModel<double>>
    {
        private readonly IMediator _mediator;

        public CollectPointCommandHandler(IUnitOfWork uow, IMediator mediator) : base(uow)
        {
            _mediator = mediator;
        }

        public override async Task<ResultModel<double>> Handle(CollectPointCommand request,
            CancellationToken cancellationToken)
        {
            var companyReward =
                await Uow.CompanyRewardRepository.GetByIdAsync(request.CompanyRewardId, cancellationToken);

            if (companyReward == null)
                return ResultModel<double>.NotFound(nameof(CompanyReward));

            AccountEntitySpecification
                accountSpec = new AccountEntitySpecification(request.AccountId, request.MemberId);

            var account = await Uow.AccountRepository.FirstOrDefaultAsync(accountSpec, cancellationToken);

            if (account == null)
                return ResultModel<double>.NotFound(nameof(Account));

            account.Balance = account.Balance + companyReward.GainedPoints;

            Uow.AccountRepository.Update(account);

            await Uow.CommitAsync(cancellationToken);

            await _mediator.Publish(new PlaceAccountRewardHistoryNotification(request.CompanyRewardId, request.AccountId,
                companyReward.GainedPoints), cancellationToken);

            return ResultModel<double>.Success(204, "points added to the account", account.Balance);
        }
    }
}