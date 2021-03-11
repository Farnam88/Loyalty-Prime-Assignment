using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;

namespace LoyaltyPrime.DataAccessLayer
{
    public interface IUnitOfWork
    {
        IRepository<Company> CompanyRepository { get; }
        IRepository<Member> MemberRepository { get; }
        IRepository<Account> AccountRepository { get; }
        IRepository<CompanyRedeemOption> CompanyRedeemOptionRepository { get; }
        IRepository<CompanyRewardOption> CompanyRewardOptionRepository { get; }
        IRepository<AccountRedeemHistory> AccountRedeemHistoryRepository { get; }
        IRepository<AccountRewardHistory> AccountRewardHistoryRepository { get; }

        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}