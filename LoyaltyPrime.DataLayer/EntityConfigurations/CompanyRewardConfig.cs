using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoyaltyPrime.DataLayer.EntityConfigurations
{
    internal class CompanyRewardConfig : IEntityTypeConfiguration<CompanyReward>
    {
        public void Configure(EntityTypeBuilder<CompanyReward> builder)
        {
            builder.ToTable("CompanyRewards");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.RewardTitle)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode()
                .HasConversion(ConfigHelpers.ConfigConverterTrim());

            builder.Property(p => p.RewardPoints)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(p => p.Company)
                .WithMany(p => p.CompanyRewards)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}