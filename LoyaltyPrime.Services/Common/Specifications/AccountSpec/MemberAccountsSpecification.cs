using LoyaltyPrime.DataAccessLayer.Infrastructure.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;

namespace LoyaltyPrime.Services.Common.Specifications.AccountSpec
{
    public class MemberAccountsSpecification : BaseSpecification<Account, MemberAccountsDto>
    {
        public MemberAccountsSpecification() : base(s =>
            new MemberAccountsDto
            {
                AccountId = s.Id,
                CompanyId = s.CompanyId,
                MemberId = s.MemberId,
                State = s.AccountState.ToString(),
                CompanyName = s.Company.Name,
                MemberName = s.Member.Name,
                Balance = s.Balance
            }, p => p.AccountState == AccountState.Active)
        {
        }

        public MemberAccountsSpecification(int accountId, int memberId) : base(s =>
            new MemberAccountsDto
            {
                AccountId = s.Id,
                CompanyId = s.CompanyId,
                MemberId = s.MemberId,
                State = s.AccountState.ToString(),
                CompanyName = s.Company.Name,
                MemberName = s.Member.Name,
                Balance = s.Balance
            }, p => p.Id == accountId && p.MemberId == memberId)
        {
        }
    }
}