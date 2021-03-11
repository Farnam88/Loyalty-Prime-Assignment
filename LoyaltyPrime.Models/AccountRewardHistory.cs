using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class AccountRewardHistory : BaseCreationModel
    {
        public int RewardOptionId { get; set; }
        public int AccountId { get; set; }
        public double RewardPoint { get; set; }

        public virtual Account Account { get; set; }
        public virtual CompanyRewardOption CompanyReward { get; set; }
    }
}