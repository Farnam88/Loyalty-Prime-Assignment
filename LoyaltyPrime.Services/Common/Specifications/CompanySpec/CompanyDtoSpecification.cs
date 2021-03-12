using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.CompanyServices.Dtos;

namespace LoyaltyPrime.Services.Common.Specifications.CompanySpec
{
    public class CompanyDtoSpecification : BaseSpecification<Company, CompanyDto>
    {
        public CompanyDtoSpecification() : base(s => new CompanyDto(s.Id, s.Name))
        {
        }
    }

    public class CompanyEntitySpecification : BaseSpecification<Company>
    {
        public CompanyEntitySpecification()
        {
            
        }
    }
}