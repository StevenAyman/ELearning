using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;
using ELearning.Domain.Purchases;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class ExamEnrollmentConfiguration : IEntityTypeConfiguration<ExamEnrollment>
{
    public void Configure(EntityTypeBuilder<ExamEnrollment> builder)
    {
        builder.ToTable("exam_enrollments");
        builder.HasIndex(e => e.Id)
            .IsUnique();

        builder.Property(e => e.StudentId)
            .HasMaxLength(50);

        builder.Property(e => e.ExamId)
            .HasMaxLength(50);

        builder.Property(e => e.PurchaseId)
            .HasMaxLength(50);

        // Relationships
        builder.HasOne<Exam>()
            .WithMany()
            .HasForeignKey(e => e.ExamId)
            .IsRequired();

        builder.HasOne<Purchase>()
            .WithOne()
            .HasForeignKey<ExamEnrollment>(e => e.PurchaseId);

        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(e => e.StudentId)
            .IsRequired();

        builder.HasKey(e => new { e.PurchaseId, e.StudentId, e.ExamId });


    }
}
