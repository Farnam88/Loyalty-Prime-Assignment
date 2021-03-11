using System;
using System.Linq.Expressions;
using LoyaltyPrime.DataAccessLayer.Infrastructure.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;

namespace LoyaltyPrime.Services.Common.Specifications.MemberSpec
{
    public class GetMembersDtoSpecification : BaseSpecification<Member, MemberDto>
    {
        public GetMembersDtoSpecification(Expression<Func<Member, bool>> criteria = null) : base(s => new MemberDto(s.Id, s.Name, s.Address),
            criteria)
        {
        }
    }
}