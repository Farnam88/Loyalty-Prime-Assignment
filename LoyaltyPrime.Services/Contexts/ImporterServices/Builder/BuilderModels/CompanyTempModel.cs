using System;

namespace LoyaltyPrime.Services.Contexts.ImporterServices.Builder.BuilderModels
{
    public class CompanyTempModel : IEquatable<CompanyTempModel>
    {
        private string _normalizedName;

        public CompanyTempModel(string name)
        {
            Name = name;
            NormalizedName = Name;
        }

        public string Name { get; set; }

        public string NormalizedName
        {
            private set { _normalizedName = value.Trim().ToUpper(); }
            get { return _normalizedName; }
        }

        public bool Equals(CompanyTempModel other)
        {
            if (other is not null)
                return NormalizedName == other.NormalizedName;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is not null)
            {
                if (obj.GetType() != this.GetType())
                    return false;
                return Equals((CompanyTempModel) obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (NormalizedName != null ? NormalizedName.GetHashCode() : 0);
        }
    }
}