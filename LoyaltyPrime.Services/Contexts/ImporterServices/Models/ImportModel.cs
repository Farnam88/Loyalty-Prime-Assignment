using System.Collections.Generic;
using LoyaltyPrime.Shared.Utilities.Extensions;
using LoyaltyPrime.Models.Bases.Enums;

namespace LoyaltyPrime.Services.Contexts.ImporterServices.Models
{
    public class ImportModel
    {
        private string _normalizedName;

        public ImportModel(string name, string address, List<ImportAccountModel> accounts)
        {
            Name = name;
            NormalizedName = name;
            Address = address;
            Accounts = accounts;
        }

        public string Name { get; set; }

        public string NormalizedName
        {
            private set { _normalizedName = value.Trim().ToUpper(); }
            get { return _normalizedName; }
        }

        public string Address { get; set; }
        public List<ImportAccountModel> Accounts { get; set; }
    }

    public class ImportAccountModel
    {
        public ImportAccountModel(string name, double balance, string status)
        {
            Name = name;
            NormalizedName = name;
            Balance = balance;
            Status = status;
        }

        private string _normalizedName;
        public string Name { get; set; }

        public string NormalizedName
        {
            private set { _normalizedName = value.Trim().ToUpper(); }
            get { return _normalizedName; }
        }

        public double Balance { get; set; }
        public string Status { get; set; }

        public AccountStatus AccountStatus()
        {
            return Status.StringToEnum<AccountStatus>();
        }
    }
}