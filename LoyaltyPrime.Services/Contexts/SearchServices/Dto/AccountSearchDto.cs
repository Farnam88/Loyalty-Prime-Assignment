
namespace LoyaltyPrime.Services.Contexts.SearchServices.Dto
{
    public class AccountSearchDto
    {
        public AccountSearchDto()
        {
            
        }
        public AccountSearchDto(int accountId, string companyName, double balance, string status)
        {
            AccountId = accountId;
            CompanyName = companyName;
            Balance = balance;
            Status = status;
        }
        public int AccountId { get; set; }
        public string CompanyName { get; set; }
        public double Balance { get; set; }
        public string Status { get; set; }
    }
}