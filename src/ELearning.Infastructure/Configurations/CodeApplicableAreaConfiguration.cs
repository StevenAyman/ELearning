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
public sealed class CodeApplicableAreaConfiguration : IEntityTypeConfiguration<CodeApplicableArea>
{
    public void Configure(EntityTypeBuilder<CodeApplicableArea> builder)
    {
        builder.ToTable("code_applicable_areas");
        builder.HasKey(ca => ca.Id);
        builder.Property(ca => ca.Id)
            .UseIdentityColumn();

        builder.Property(ca => ca.Type)
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(type => type.Value, value => new TypeName(value));
    }
}
