using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoyaltyPrime.DataLayer.EntityConfigurations
{
    internal class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Balance)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.AccountStatus)
                .IsRequired()
                .HasDefaultValue(AccountStatus.Active);

            builder.Property(p => p.RowVersion)
                .IsRequired()
                .IsRowVersion()
                .ValueGeneratedOnAddOrUpdate();

            builder.HasIndex(i => new
                {
                    i.CompanyId,
                    i.MemberId
                }).IsUnique()
                .HasDatabaseName("Uix_CompanyMemberAccountIndex");
        }
    }
}