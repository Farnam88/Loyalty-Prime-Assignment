using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer.Infrastructure.Repositories;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataLayer;
using LoyaltyPrime.Models;

namespace LoyaltyPrime.DataAccessLayer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LoyaltyPrimeContext _context;
        private IRepository<Company> _companyRepository;
        private IRepository<Member> _memberRepository;
        private IRepository<Account> _accountRepository;
        private IRepository<CompanyRedeemOption> _companyRedeemOptionRepository;
        private IRepository<CompanyRewardOption> _companyRewardOptionRepository;
        private IRepository<AccountRedeemHistory> _accountRedeemHistoryRepository;
        private IRepository<AccountRewardHistory> _accountRewardHistoryRepository;

        public UnitOfWork(LoyaltyPrimeContext context)
        {
            _context = context;
        }

        public IRepository<Company> CompanyRepository
        {
            get { return _companyRepository ??= new Repository<Company>(_context); }
        }

        public IRepository<Member> MemberRepository
        {
            get { return _memberRepository ??= new Repository<Member>(_context); }
        }

        public IRepository<Account> AccountRepository
        {
            get { return _accountRepository ??= new Repository<Account>(_context); }
        }

        public IRepository<CompanyRedeemOption> CompanyRedeemOptionRepository
        {
            get { return _companyRedeemOptionRepository ??= new Repository<CompanyRedeemOption>(_context); }
        }

        public IRepository<CompanyRewardOption> CompanyRewardOptionRepository
        {
            get { return _companyRewardOptionRepository ??= new Repository<CompanyRewardOption>(_context); }
        }

        public IRepository<AccountRedeemHistory> AccountRedeemHistoryRepository
        {
            get { return _accountRedeemHistoryRepository ??= new Repository<AccountRedeemHistory>(_context); }
        }

        public IRepository<AccountRewardHistory> AccountRewardHistoryRepository
        {
            get { return _accountRewardHistoryRepository ??= new Repository<AccountRewardHistory>(_context); }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}