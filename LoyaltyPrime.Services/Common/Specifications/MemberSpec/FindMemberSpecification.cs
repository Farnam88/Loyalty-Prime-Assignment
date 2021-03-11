using LoyaltyPrime.DataAccessLayer.Infrastructure.Specifications;
using LoyaltyPrime.Models;

namespace LoyaltyPrime.Services.Common.Specifications.MemberSpec
{
    public class FindMemberSpecification : BaseSpecification<Member>
    {
        public FindMemberSpecification(int id) : base(p => p.Id == id)
        {
        }
    }
}