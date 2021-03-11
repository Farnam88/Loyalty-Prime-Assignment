using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoyaltyPrime.DataLayer.EntityConfigurations
{
    internal class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode()
                .HasConversion(ConfigHelpers.ConfigConverterTrim());

            builder.Property(b => b.NormalizedName)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode()
                .HasConversion(ConfigHelpers.ConfigConverterTrimUpper(), ConfigHelpers.ConfigComParer());

            builder.HasMany(p => p.Accounts)
                .WithOne(p => p.Company)
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(i => i.NormalizedName)
                .IsUnique(false)
                .HasDatabaseName("IX_NormalizedName");
        }
    }
}