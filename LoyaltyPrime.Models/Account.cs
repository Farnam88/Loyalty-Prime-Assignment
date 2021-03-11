using System.Collections.Generic;
using LoyaltyPrime.Models.Bases.CommonEntities;
using LoyaltyPrime.Models.Bases.Enums;

namespace LoyaltyPrime.Models
{
    public class Account : BaseConcurrentModel
    {
        public Account()
        {
        }

        public Account(int memberId, int companyId, double balance, AccountState accountState)
        {
            MemberId = memberId;
            CompanyId = companyId;
            Balance = balance;
            AccountState = accountState;
            AccountRedeemHistories = new HashSet<AccountRedeemHistory>();
            AccountRewardHistories = new HashSet<AccountRewardHistory>();
        }

        public int MemberId { get; set; }
        public int CompanyId { get; set; }
        public double Balance { get; set; }
        public AccountState AccountState { get; set; }

        public virtual Member Member { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<AccountRedeemHistory> AccountRedeemHistories { get; set; }
        public virtual ICollection<AccountRewardHistory> AccountRewardHistories { get; set; }
    }
}