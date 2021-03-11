using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoyaltyPrime.DataLayer.EntityConfigurations
{
    internal class AccountRewardHistoryConfig : IEntityTypeConfiguration<AccountRewardHistory>
    {
        public void Configure(EntityTypeBuilder<AccountRewardHistory> builder)
        {
            builder.ToTable("AccountRewardHistories");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.RewardPoints)
                .IsRequired()
                .HasDefaultValue(0);


            builder.HasOne(p => p.Account)
                .WithMany(p => p.AccountRewardHistories)
                .HasForeignKey(f => f.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.CompanyReward)
                .WithMany(p => p.AccountRewardHistories)
                .HasForeignKey(f => f.CompanyRewardId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}