#nullable enable
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Dto;

namespace LoyaltyPrime.Services.Common.Specifications.CompanyRedeemSpec
{
    public class CompanyRedeemsSpecification : BaseSpecification<CompanyRedeem, CompanyRedeemDto>
    {
        public CompanyRedeemsSpecification(int companyId) : base(s =>
                new CompanyRedeemDto(s.CompanyId, s.Company.Name, s.RedeemTitle, s.RedeemPoints,s.Id),
            p => p.CompanyId == companyId)
        {
        }
    }
}