using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;

namespace LoyaltyPrime.Services.Common.Specifications.AccountSpec
{
    public class AccountEntitySpecification : BaseSpecification<Account>
    {
        public AccountEntitySpecification(int accountId, int memberId) : base(p =>
            p.Id == accountId && p.MemberId == memberId)
        {
        }

        public AccountEntitySpecification()
        {
            
        }
    }
}