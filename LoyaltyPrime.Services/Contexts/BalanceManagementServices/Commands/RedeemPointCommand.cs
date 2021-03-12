using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Common.Base;
using LoyaltyPrime.Services.Common.Specifications.AccountSpec;
using LoyaltyPrime.Services.Contexts.AccountRedeemHistoryServices.Notifications;
using LoyaltyPrime.Services.Contexts.AccountRewardHistoryServices.Notifications;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.BalanceManagementServices.Commands
{
    public class RedeemPointCommand : IRequest<ResultModel<double>>
    {
        public int AccountId { get; set; }
        public int MemberId { get; set; }
        public int CompanyRedeemId { get; set; }

        public RedeemPointCommand(int accountId, int memberId, int companyRedeemId)
        {
            AccountId = accountId;
            MemberId = memberId;
            CompanyRedeemId = companyRedeemId;
        }
    }

    public class RedeemPointCommandHandler : BaseRequestHandler<RedeemPointCommand, ResultModel<double>>
    {
        private readonly IMediator _mediator;

        public RedeemPointCommandHandler(IUnitOfWork uow, IMediator mediator) : base(uow)
        {
            _mediator = mediator;
        }

        public override async Task<ResultModel<double>> Handle(RedeemPointCommand request,
            CancellationToken cancellationToken)
        {
            var companyRedeem =
                await Uow.CompanyRedeemRepository.GetByIdAsync(request.CompanyRedeemId, cancellationToken);

            if (companyRedeem == null)
                return ResultModel<double>.NotFound(nameof(CompanyRedeem));

            AccountEntitySpecification
                accountSpec = new AccountEntitySpecification(request.AccountId, request.MemberId);

            var account = await Uow.AccountRepository.FirstOrDefaultAsync(accountSpec, cancellationToken);

            if (account == null)
                return ResultModel<double>.NotFound(nameof(Account));
            if (account.Balance < companyRedeem.RedeemPoints)
                return ResultModel<double>.Fail(404, "Insufficient balance in your account",
                    ErrorTypes.LogicalError, new Dictionary<string, string>
                    {
                        {"Current Balance", $"{account.Balance}"},
                        {"Requested Redeem", $"{companyRedeem.RedeemPoints}"}
                    });
            if (account.AccountState == AccountState.Inactive)
                return ResultModel<double>.Fail(404, "You can not redeem from an inactive account");

            account.Balance = account.Balance - companyRedeem.RedeemPoints;

            Uow.AccountRepository.Update(account);

            await Uow.CommitAsync(cancellationToken);

            await _mediator.Publish(new PlaceAccountRedeemHistoryNotification(request.CompanyRedeemId,
                request.AccountId,
                companyRedeem.RedeemPoints), cancellationToken);

            return ResultModel<double>.Success(204, "points added to the account", account.Balance);
        }
    }
}