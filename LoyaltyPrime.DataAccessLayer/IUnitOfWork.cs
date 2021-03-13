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
        IRepository<CompanyRedeem> CompanyRedeemRepository { get; }
        IRepository<CompanyReward> CompanyRewardRepository { get; }
        IRepository<AccountRedeemHistory> AccountRedeemHistoryRepository { get; }
        IRepository<AccountRewardHistory> AccountRewardHistoryRepository { get; }
        ISearchRepository SearchRepository { get; }

        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}