using System.Collections.Generic;
using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class Company : BaseModel
    {
        public Company(string name)
        {
            Name = name;
            NormalizedName = name;
            Accounts = new HashSet<Account>();
            CompanyRewardOptions = new HashSet<CompanyRewardOption>();
            CompanyRedeemOptions = new HashSet<CompanyRedeemOption>();
        }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<CompanyRewardOption> CompanyRewardOptions { get; set; }
        public virtual ICollection<CompanyRedeemOption> CompanyRedeemOptions { get; set; }
    }
}