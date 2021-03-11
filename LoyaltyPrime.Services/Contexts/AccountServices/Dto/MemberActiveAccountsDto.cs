namespace LoyaltyPrime.Services.Contexts.AccountServices.Dto
{
    public class MemberActiveAccountsDto
    {
        public int AccountId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public double Balance { get; set; }
        public string State { get; set; }
    }
}