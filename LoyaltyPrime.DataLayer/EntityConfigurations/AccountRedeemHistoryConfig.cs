using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoyaltyPrime.DataLayer.EntityConfigurations
{
    internal class AccountRedeemHistoryConfig : IEntityTypeConfiguration<AccountRedeemHistory>
    {
        public void Configure(EntityTypeBuilder<AccountRedeemHistory> builder)
        {
            builder.ToTable("AccountRewardHistories");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.RedeemPoints)
                .IsRequired()
                .HasDefaultValue(0);


            builder.HasOne(p => p.Account)
                .WithMany(p => p.AccountRedeemHistories)
                .HasForeignKey(f => f.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.CompanyRedeem)
                .WithMany(p => p.AccountRedeemHistories)
                .HasForeignKey(f => f.CompanyRedeemId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}