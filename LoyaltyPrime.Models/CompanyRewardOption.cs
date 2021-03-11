using System.Collections.Generic;
using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class CompanyRewardOption : BaseModel
    {
        public CompanyRewardOption()
        {
        }

        public CompanyRewardOption(string rewardTitle, int companyId, double rewardPoint)
        {
            RewardTitle = rewardTitle;
            CompanyId = companyId;
            RewardPoint = rewardPoint;
            AccountRewardHistories = new HashSet<AccountRewardHistory>();
        }

        public string RewardTitle { get; set; }
        public int CompanyId { get; set; }
        public double RewardPoint { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<AccountRewardHistory> AccountRewardHistories { get; set; }
    }
}