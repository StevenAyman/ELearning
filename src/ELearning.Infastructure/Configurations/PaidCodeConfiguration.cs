using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Purchases;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class PaidCodeConfiguration : IEntityTypeConfiguration<PaidCode>
{
    public void Configure(EntityTypeBuilder<PaidCode> builder)
    {
        builder.ToTable("paid_codes");
        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.Id)
            .HasMaxLength(50);

        builder.Property(pc => pc.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(pc => pc.Code)
            .IsUnique();

        builder.Property(pc => pc.Balance)
            .HasColumnType("decimal(18,2)")
            .HasConversion(balance => balance.Amount, value => new Money(value));

        builder.Property(pc => pc.Status)
            .HasConversion(
                s => s.ToString(),
                v => Enum.Parse<PaidCodeStatus>(v)
            );

        builder.Property<uint>("row_version")
            .IsRowVersion();

        // Relationships // student may be use multiple code but code is used by only one student
        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(pc => pc.StudentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(pc => pc.StudentId)
            .HasMaxLength(50);

        builder.HasIndex(pc => pc.StudentId);
    }
}
