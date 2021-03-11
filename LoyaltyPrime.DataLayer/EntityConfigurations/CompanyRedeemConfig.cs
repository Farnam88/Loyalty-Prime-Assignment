using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoyaltyPrime.DataLayer.EntityConfigurations
{
    internal class CompanyRedeemConfig : IEntityTypeConfiguration<CompanyRedeem>
    {
        public void Configure(EntityTypeBuilder<CompanyRedeem> builder)
        {
            builder.ToTable("CompanyRedeems");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.RedeemTitle)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode()
                .HasConversion(ConfigHelpers.ConfigConverterTrim());

            builder.Property(p => p.RedeemPoints)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(p => p.Company)
                .WithMany(p => p.CompanyRedeems)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}