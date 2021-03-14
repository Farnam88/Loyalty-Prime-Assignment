using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;

namespace LoyaltyPrime.Services.Common.Specifications.AccountSpec
{
    public class AccountsDtoSpecification : BaseSpecification<Account, AccountDto>
    {
        public AccountsDtoSpecification(int memberId) : base(s =>
            new AccountDto
            {
                AccountId = s.Id,
                CompanyId = s.CompanyId,
                MemberId = s.MemberId,
                Status = s.AccountStatus.ToString().ToUpper(),
                CompanyName = s.Company.Name,
                MemberName = s.Member.Name,
                Balance = s.Balance
            }, p => p.MemberId == memberId)
        {
        }

        public AccountsDtoSpecification(int memberId,int accountId) : base(s =>
            new AccountDto
            {
                AccountId = s.Id,
                CompanyId = s.CompanyId,
                MemberId = s.MemberId,
                Status = s.AccountStatus.ToString().ToUpper(),
                CompanyName = s.Company.Name,
                MemberName = s.Member.Name,
                Balance = s.Balance
            }, p => p.Id == accountId && p.MemberId == memberId)
        {
        }
    }
}