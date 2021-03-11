using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class AccountRedeemHistory : BaseCreationModel
    {
        public int RedeemOptionId { get; set; }
        public int AccountId { get; set; }
        public double RedeemPoint { get; set; }

        public virtual Account Account { get; set; }
        public virtual CompanyRedeemOption CompanyRedeemOption { get; set; }
    }
}