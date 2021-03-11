using System.Collections.Generic;
using LoyaltyPrime.Models;

namespace LoyaltyPrime.Infrastructure.Tests.Helpers
{
    public class EntityGenerator
    {
        public static Company CreateCompany()
        {
            return new Company("Burger King")
            {
                Id = 1
            };
        }

        public static IList<Company> CreateCompanies()
        {
            return new List<Company>
            {
                new Company("Burger King")
                {
                    Id = 1,
                },
                new Company("Fitness First")
                {
                    Id = 2
                }
            };
        }
    }
}