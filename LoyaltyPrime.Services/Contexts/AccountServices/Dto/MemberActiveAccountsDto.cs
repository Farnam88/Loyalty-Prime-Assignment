namespace LoyaltyPrime.Services.Contexts.AccountServices.Dto
{
    public class MemberAccountsDto
    {
        public MemberAccountsDto()
        {
            
        }
        public MemberAccountsDto(int accountId, int companyId, int memberId, string companyName, string memberName, double balance, string state)
        {
            AccountId = accountId;
            CompanyId = companyId;
            MemberId = memberId;
            CompanyName = companyName;
            MemberName = memberName;
            Balance = balance;
            State = state;
        }
        public int AccountId { get; set; }
        public int CompanyId { get; set; }
        public int MemberId { get; set; }
        public string CompanyName { get; set; }
        public string MemberName { get; set; }
        public double Balance { get; set; }
        public string State { get; set; }
    }
}