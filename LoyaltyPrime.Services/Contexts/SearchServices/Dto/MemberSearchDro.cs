using System.Collections.Generic;

namespace LoyaltyPrime.Services.Contexts.SearchServices.Dto
{
    public class MemberSearchDro
    {
        public MemberSearchDro()
        {
            
        }
        public MemberSearchDro(int id, string name, string address, List<AccountSearchDto> accounts)
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
        public IEnumerable<AccountSearchDto> Accounts { get; set; }
    }
}