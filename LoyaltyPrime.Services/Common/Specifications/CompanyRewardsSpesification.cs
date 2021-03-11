#nullable enable
using LoyaltyPrime.DataAccessLayer.Infrastructure.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Dto;

namespace LoyaltyPrime.Services.Common.Specifications
{
    public class CompanyRewardsSpecification : BaseSpecification<CompanyReward, CompanyRewardDto>
    {
        public CompanyRewardsSpecification(int companyId) : base(s => new CompanyRewardDto
        {
            CompanyId = s.CompanyId,
            CompanyName = s.Company.Name,
            RewardPoints = s.RewardPoints,
            RewardTitle = s.RewardTitle
        }, p => p.CompanyId == companyId)
        {
        }
    }
}