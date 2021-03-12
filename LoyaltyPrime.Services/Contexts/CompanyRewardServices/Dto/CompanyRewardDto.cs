namespace LoyaltyPrime.Services.Contexts.CompanyRewardServices.Dto
{
    public class CompanyRewardDto
    {
        public CompanyRewardDto(int companyId, string companyName, string rewardTitle, double rewardPoints,
            int companyRewardId)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            RewardTitle = rewardTitle;
            RewardPoints = rewardPoints;
            CompanyRewardId = companyRewardId;
        }

        public int CompanyRewardId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string RewardTitle { get; set; }
        public double RewardPoints { get; set; }
    }
}