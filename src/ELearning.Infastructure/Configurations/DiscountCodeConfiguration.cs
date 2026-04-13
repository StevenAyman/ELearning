using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using ELearning.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
{
    public void Configure(EntityTypeBuilder<DiscountCode> builder)
    {
        builder.ToTable("discount_codes");
        builder.HasKey(dc => dc.Id);
        builder.Property(dc => dc.Id)
            .HasMaxLength(50);

        builder.Property(dc => dc.Code)
            .HasMaxLength(50);

        builder.HasIndex(dc => dc.Code)
            .IsUnique();

        builder.Property(dc => dc.DiscountType)
            .HasConversion(
                type => type.ToString(),
                type => Enum.Parse<DiscountAmountType>(type)
            );

        builder.Property(dc => dc.ExpireType)
            .HasConversion(
                type => type.ToString(),
                type => Enum.Parse<DiscountExpirationType>(type)
            );

        builder.Property(dc => dc.DiscountAmount)
            .HasColumnType("decimal(18,2)")
            .HasConversion(value => value.Amount, value => new Money(value));

        builder.Property(dc => dc.CountLimit)
            .IsRequired(false);

        builder.OwnsOne(dc => dc.ExpirePeriod, expireBuilder =>
        {
            expireBuilder.Property(d => d.StartDate)
            .HasColumnName("expire_start_date")
            .IsRequired(false);

            expireBuilder.Property(d => d.EndDate)
            .HasColumnName("expire_end_date")
            .IsRequired(false);
        });

        builder.Property<uint>("row_version")
            .IsRowVersion();
    }
}
