#nullable enable
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Dto;

namespace LoyaltyPrime.Services.Common.Specifications.CompanyRewardSpec
{
    public class CompanyRewardsSpecification : BaseSpecification<CompanyReward, CompanyRewardDto>
    {
        public CompanyRewardsSpecification(int companyId) : base(s =>
                new CompanyRewardDto(s.CompanyId, s.Company.Name, s.RewardTitle, s.RewardPoints, s.Id),
            p => p.CompanyId == companyId)
        {
        }
    }
}