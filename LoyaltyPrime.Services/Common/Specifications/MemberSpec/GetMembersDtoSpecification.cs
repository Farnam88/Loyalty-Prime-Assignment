using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;

namespace LoyaltyPrime.Services.Common.Specifications.MemberSpec
{
    public class GetMembersDtoSpecification : BaseSpecification<Member, MemberDto>
    {
        public GetMembersDtoSpecification() : base(s => new MemberDto(s.Id, s.Name, s.Address))
        {
        }
    }
}