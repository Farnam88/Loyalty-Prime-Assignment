namespace LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Dto
{
    public class CompanyRedeemDto
    {
        public CompanyRedeemDto(int companyId, string companyName, string redeemTitle, double redeemPoints,
            int companyRedeemId)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            RedeemTitle = redeemTitle;
            RedeemPoints = redeemPoints;
            CompanyRedeemId = companyRedeemId;
        }

        public int CompanyId { get; set; }
        public int CompanyRedeemId { get; set; }
        public string CompanyName { get; set; }
        public string RedeemTitle { get; set; }
        public double RedeemPoints { get; set; }
    }
}