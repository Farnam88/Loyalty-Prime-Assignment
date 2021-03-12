namespace LoyaltyPrime.Services.Contexts.AccountServices.Dto
{
    public class AccountDto
    {
        public AccountDto()
        {
            
        }
        public AccountDto(int accountId, int companyId, int memberId, string companyName, string memberName, double balance, string status)
        {
            AccountId = accountId;
            CompanyId = companyId;
            MemberId = memberId;
            CompanyName = companyName;
            MemberName = memberName;
            Balance = balance;
            Status = status;
        }
        public int AccountId { get; set; }
        public int CompanyId { get; set; }
        public int MemberId { get; set; }
        public string CompanyName { get; set; }
        public string MemberName { get; set; }
        public double Balance { get; set; }
        public string Status { get; set; }
    }
}