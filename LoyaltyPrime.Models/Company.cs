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
            CompanyRewards = new HashSet<CompanyReward>();
            CompanyRedeems = new HashSet<CompanyRedeem>();
        }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<CompanyReward> CompanyRewards { get; set; }
        public virtual ICollection<CompanyRedeem> CompanyRedeems { get; set; }
    }
}