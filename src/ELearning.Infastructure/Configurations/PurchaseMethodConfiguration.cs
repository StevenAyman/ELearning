using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class PurchaseMethodConfiguration : IEntityTypeConfiguration<PurchaseMethod>
{
    public void Configure(EntityTypeBuilder<PurchaseMethod> builder)
    {
        builder.ToTable("purchase_methods");
        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Id)
            .HasMaxLength(50);

        builder.Property(pm => pm.Type)
            .HasConversion(
                type => type.ToString(),
                value => Enum.Parse<PaymentType>(value)
            );
    }
}
