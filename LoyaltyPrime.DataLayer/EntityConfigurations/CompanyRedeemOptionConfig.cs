using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoyaltyPrime.DataLayer.EntityConfigurations
{
    internal class CompanyRedeemOptionConfig : IEntityTypeConfiguration<CompanyRedeemOption>
    {
        public void Configure(EntityTypeBuilder<CompanyRedeemOption> builder)
        {
            builder.ToTable("CompanyRedeemOptions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.RedeemTitle)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode()
                .HasConversion(ConfigHelpers.ConfigConverterTrim());

            builder.Property(p => p.RedeemPoint)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(p => p.Company)
                .WithMany(p => p.CompanyRedeemOptions)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}