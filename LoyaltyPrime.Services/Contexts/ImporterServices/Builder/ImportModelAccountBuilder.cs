using System.Collections.Generic;
using System.Linq;
using LoyaltyPrime.Shared.Utilities.Extensions;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.ImporterServices.Models;

namespace LoyaltyPrime.Services.Contexts.ImporterServices.Builder
{
    public class ImportModelAccountBuilder
    {
        private readonly List<Company> _companies;
        private readonly List<Member> _members;
        private readonly List<ImportModel> _importModels;
        private readonly List<Account> _accounts;


        public ImportModelAccountBuilder(List<Company> companies, List<Member> members, List<ImportModel> importModels)
        {
            _companies = companies;
            _members = members;
            _importModels = importModels;
            _accounts = new List<Account>();
        }

        public ImportModelAccountBuilder BuildAccounts()
        {
            foreach (var member in _importModels)
            {
                foreach (var account in member.Accounts)
                {
                    var selectedCompany =
                        _companies.FirstOrDefault(f =>
                            StringExtensions.ToNormalize(f.NormalizedName) == account.NormalizedName);
                    var selectedMember =
                        _members.FirstOrDefault(f => f.NormalizedName.ToNormalize() == member.NormalizedName);
                    if (selectedCompany is not null && selectedMember is not null)
                        _accounts.Add(new Account(selectedMember, selectedCompany, account.Balance,
                            account.AccountStatus()));
                }
            }

            return this;
        }

        public List<Account> GetAccounts()
        {
            Preconditions.CheckNull(_accounts);
            return _accounts;
        }
    }
}