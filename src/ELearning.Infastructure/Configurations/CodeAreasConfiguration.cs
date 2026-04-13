using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class CodeAreasConfiguration : IEntityTypeConfiguration<CodeAreas>
{
    public void Configure(EntityTypeBuilder<CodeAreas> builder)
    {
        builder.ToTable("code_areas");
        builder.HasKey(ca => ca.Id);
        builder.Property(ca => ca.Id)
            .UseIdentityColumn();

        builder.Property(ca => ca.CodeId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ca => ca.TargetId)
            .IsRequired(false)
            .HasMaxLength(50);

        // Relationships

        builder.HasOne<CodeApplicableArea>()
            .WithMany()
            .HasForeignKey(ca => ca.AppplicableAreaId)
            .IsRequired();

        builder.HasOne<DiscountCode>()
            .WithMany()
            .HasForeignKey(ca => ca.CodeId);

        builder.HasIndex(ca => new { ca.AppplicableAreaId, ca.CodeId });


    }
}
