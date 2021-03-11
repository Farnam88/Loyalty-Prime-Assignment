using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoyaltyPrime.DataLayer.EntityConfigurations
{
    internal class CompanyRewardOptionConfig : IEntityTypeConfiguration<CompanyRewardOption>
    {
        public void Configure(EntityTypeBuilder<CompanyRewardOption> builder)
        {
            builder.ToTable("CompanyRewardOptions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.RewardTitle)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode()
                .HasConversion(ConfigHelpers.ConfigConverterTrim());

            builder.Property(p => p.RewardPoint)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(p => p.Company)
                .WithMany(p => p.CompanyRewardOptions)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}