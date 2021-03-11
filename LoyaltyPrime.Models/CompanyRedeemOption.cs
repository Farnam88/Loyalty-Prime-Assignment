using System.Collections.Generic;
using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class CompanyRedeemOption : BaseModel
    {
        public CompanyRedeemOption(double redeemPoint)
        {
            RedeemPoint = redeemPoint;
        }

        public CompanyRedeemOption(string redeemTitle, int companyId, double redeemPoint)
        {
            RedeemTitle = redeemTitle;
            CompanyId = companyId;
            RedeemPoint = redeemPoint;
            RedeemPoint = redeemPoint;
            AccountRedeemHistories = new HashSet<AccountRedeemHistory>();
        }

        public string RedeemTitle { get; set; }
        public int CompanyId { get; set; }
        public double RedeemPoint { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<AccountRedeemHistory> AccountRedeemHistories { get; set; }
    }
}