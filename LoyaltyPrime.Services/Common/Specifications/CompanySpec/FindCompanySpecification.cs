using LoyaltyPrime.DataAccessLayer.Infrastructure.Specifications;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;

namespace LoyaltyPrime.Services.Common.Specifications.CompanySpec
{
    public class FindCompanySpecification : BaseSpecification<Company>
    {
        public FindCompanySpecification(int id) : base(p => p.Id == id)
        {
        }
    }
}