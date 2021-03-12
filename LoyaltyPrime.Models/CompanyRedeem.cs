using System.Collections.Generic;
using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class CompanyRedeem : BaseModel
    {
        public CompanyRedeem(double redeemPoint)
        {
            RedeemPoints = redeemPoint;
        }

        public CompanyRedeem(string redeemTitle, int companyId, double redeemPoint)
        {
            RedeemTitle = redeemTitle;
            CompanyId = companyId;
            RedeemPoints = redeemPoint;
            AccountRedeemHistories = new HashSet<AccountRedeemHistory>();
        }

        public string RedeemTitle { get; set; }
        public int CompanyId { get; set; }
        public double RedeemPoints { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<AccountRedeemHistory> AccountRedeemHistories { get; set; }
    }
}