using System.Collections.Generic;

namespace LoyaltyPrime.Services.Contexts.SearchServices.Dto
{
    public class MemberSearchDto
    {
        public MemberSearchDto()
        {
            
        }
        public MemberSearchDto(int id, string name, string address, List<AccountSearchDto> accounts)
        {
            Id = id;
            Name = name;
            Address = address;
            Accounts = accounts;
            Accounts = new List<AccountSearchDto>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<AccountSearchDto> Accounts { get; set; }
    }
}