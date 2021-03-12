namespace LoyaltyPrime.Services.Contexts.CompanyServices.Dtos
{
    public class CompanyDto
    {
        public CompanyDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}