using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class AccountRedeemHistory : BaseCreationModel
    {
        public AccountRedeemHistory()
        {
        }

        public AccountRedeemHistory(int companyRedeemId, int accountId, double redeemPoints)
        {
            CompanyRedeemId = companyRedeemId;
            AccountId = accountId;
            RedeemPoints = redeemPoints;
        }

        public int CompanyRedeemId { get; set; }
        public int AccountId { get; set; }
        public double RedeemPoints { get; set; }

        public virtual Account Account { get; set; }
        public virtual CompanyRedeem CompanyRedeem { get; set; }
    }
}