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

        public Account(Member member, Company company, double balance, AccountStatus accountStatus)
        {
            Member = member;
            Company = company;
            Balance = balance;
            AccountStatus = accountStatus;
        }

        public Account(int memberId, int companyId, double balance, AccountStatus accountStatus)
        {
            MemberId = memberId;
            CompanyId = companyId;
            Balance = balance;
            AccountStatus = accountStatus;
            AccountRedeemHistories = new HashSet<AccountRedeemHistory>();
            AccountRewardHistories = new HashSet<AccountRewardHistory>();
        }

        public int MemberId { get; set; }
        public int CompanyId { get; set; }
        public double Balance { get; set; }
        public AccountStatus AccountStatus { get; set; }

        public virtual Member Member { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<AccountRedeemHistory> AccountRedeemHistories { get; set; }
        public virtual ICollection<AccountRewardHistory> AccountRewardHistories { get; set; }
    }
}