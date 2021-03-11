using System.Collections.Generic;
using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class CompanyReward : BaseModel
    {
        public CompanyReward()
        {
        }

        public CompanyReward(string rewardTitle, int companyId, double rewardPoint)
        {
            RewardTitle = rewardTitle;
            CompanyId = companyId;
            RewardPoints = rewardPoint;
            AccountRewardHistories = new HashSet<AccountRewardHistory>();
        }

        public string RewardTitle { get; set; }
        public int CompanyId { get; set; }
        public double RewardPoints { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<AccountRewardHistory> AccountRewardHistories { get; set; }
    }
}