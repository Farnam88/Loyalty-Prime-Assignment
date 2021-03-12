using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class AccountRewardHistory : BaseCreationModel
    {
        public AccountRewardHistory()
        {
            
        }
        public AccountRewardHistory(int companyRewardId, int accountId, double rewardPoints)
        {
            CompanyRewardId = companyRewardId;
            AccountId = accountId;
            RewardPoints = rewardPoints;
        }
        public int CompanyRewardId { get; set; }
        public int AccountId { get; set; }
        public double RewardPoints { get; set; }

        public virtual Account Account { get; set; }
        public virtual CompanyReward CompanyReward { get; set; }
    }
}