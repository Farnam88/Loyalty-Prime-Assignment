using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Extensions;
using LoyaltyPrime.DataLayer.EntityConfigurations;
using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyPrime.DataLayer
{
    public class LoyaltyPrimeContext : DbContext
    {
        private DbSet<Company> Companies { get; set; }
        private DbSet<Account> Accounts { get; set; }
        private DbSet<Member> Members { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Preconditions.CheckNull(optionsBuilder, nameof(DbContextOptionsBuilder));
            optionsBuilder.UseInMemoryDatabase("loyalty-prime-db");
        }

        public LoyaltyPrimeContext()
        {
            
        }
        public LoyaltyPrimeContext(DbContextOptionsBuilder optionsBuilder)
        {
            Preconditions.CheckNull(optionsBuilder, nameof(DbContextOptionsBuilder));
            optionsBuilder.UseInMemoryDatabase("loyalty-prime-db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Preconditions.CheckNull(modelBuilder, nameof(ModelBuilder));

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompanyConfig).Assembly);
        }
    }
}