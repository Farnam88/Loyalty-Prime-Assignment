using System;
using System.Linq.Expressions;
using LoyaltyPrime.DataAccessLayer.Infrastructure.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;

namespace LoyaltyPrime.Services.Common.Specifications.AccountSpec
{
    public class MemberActiveAccountsSpecification : BaseSpecification<Account, MemberActiveAccountsDto>
    {
        public MemberActiveAccountsSpecification() : base(s =>
            new MemberActiveAccountsDto
            {
                AccountId = s.Id,
                CompanyId = s.CompanyId,
                State = s.AccountState.ToString(),
                CompanyName = s.Company.Name,
                Balance = s.Balance
            }, p => p.AccountState == AccountState.Active)
        {
        }
    }
}