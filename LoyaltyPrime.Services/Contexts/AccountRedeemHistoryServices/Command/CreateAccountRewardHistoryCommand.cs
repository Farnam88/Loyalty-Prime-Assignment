using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Base;
using MediatR;

namespace LoyaltyPrime.Services.Contexts.AccountRedeemHistoryServices.Command
{
    public class CreateAccountRedeemHistoryCommand : IRequest<ResultModel<int>>
    {
        public CreateAccountRedeemHistoryCommand()
        {
        }

        public CreateAccountRedeemHistoryCommand(int companyRedeemId, int accountId, double redeemPoints)
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
        CreateAccountRedeemHistoryCommandHandler : BaseRequestHandler<CreateAccountRedeemHistoryCommand,
            ResultModel<int>>
    {
        public CreateAccountRedeemHistoryCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task<ResultModel<int>> Handle(CreateAccountRedeemHistoryCommand request,
            CancellationToken cancellationToken)
        {
            var accountRedeemHistory =
                new AccountRedeemHistory(request.CompanyRedeemId, request.AccountId, request.RedeemPoints);
            await Uow.AccountRedeemHistoryRepository.AddAsync(accountRedeemHistory, cancellationToken);
            await Uow.CommitAsync(cancellationToken);
            return ResultModel<int>.Success(201, "Account redeem history created", accountRedeemHistory.Id);
        }
    }
}