using System.Collections.Generic;
using System.Linq;
using LoyaltyPrime.Shared.Utilities.Extensions;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.ImporterServices.Builder.BuilderModels;
using LoyaltyPrime.Services.Contexts.ImporterServices.Models;

namespace LoyaltyPrime.Services.Contexts.ImporterServices.Builder
{
    public class ImportModelCompanyBuilder
    {
        private List<ImportModel> _importmodels;
        private readonly List<Company> _companies;

        public ImportModelCompanyBuilder(List<ImportModel> importmodels)
        {
            _importmodels = importmodels;
            _companies = new List<Company>();
        }

        public ImportModelCompanyBuilder BuildCompanies()
        {
            var selectedCompanies = _importmodels.SelectMany(s => s.Accounts)
                .Select(s => new CompanyTempModel(s.Name))
                .ToList();

            if (selectedCompanies.Any())
            {
                HashSet<CompanyTempModel> distinctSet = new HashSet<CompanyTempModel>(selectedCompanies);
                var distinctCompanies = distinctSet.Select(s => new Company(s.Name)).ToList();
                _companies.AddRange(distinctCompanies);
            }

            return this;
        }

        public List<Company> GetCompanies()
        {
            Preconditions.CheckNull(_companies);
            return _companies;
        }
    }
}