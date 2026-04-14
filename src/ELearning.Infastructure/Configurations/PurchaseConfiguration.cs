using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Discounts;
using ELearning.Domain.Exams;
using ELearning.Domain.Purchases;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("purchases");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasMaxLength(50);

        builder.Property(p => p.StudentId)
            .HasMaxLength(50);
        
        builder.Property(p => p.SessionId)
            .HasMaxLength(50);

        builder.Property(p => p.ExamId)
            .HasMaxLength(50);

        builder.HasIndex(p => new { p.StudentId, p.ExamId });
        builder.HasIndex(p => new { p.StudentId, p.SessionId });

        builder.Property(p => p.TotalPaid)
            .HasColumnType("decimal(18,2)")
            .HasConversion(totalpaid => totalpaid.Amount, value => new Money(value));

        builder.Property(p => p.Status)
            .HasConversion(
                s => s.ToString(),
                v => Enum.Parse<PaymentStatus>(v)
            );

        builder.Property(p => p.CodeId)
            .HasMaxLength(50);

        // Relationships

        builder.HasOne(p => p.PaymentMethod)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<DiscountCode>()
            .WithMany()
            .HasForeignKey(p => p.CodeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(p => p.CodeId);

        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Session>()
            .WithMany()
            .HasForeignKey(p => p.SessionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Exam>()
            .WithMany()
            .HasForeignKey(p => p.ExamId)
            .OnDelete(DeleteBehavior.NoAction);


    }
}
