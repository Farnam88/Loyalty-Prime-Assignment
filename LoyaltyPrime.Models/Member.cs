using System.Collections.Generic;
using LoyaltyPrime.Models.Bases.CommonEntities;

namespace LoyaltyPrime.Models
{
    public class Member : BaseCreationModel
    {
        public Member(string name, string address = "")
        {
            Name = name;
            NormalizedName = name;
            Address = address;
        }

        public string Name { get; set; }

        public string NormalizedName { get; set; }


        public string Address { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}