using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;
using ELearning.Domain.Instructors;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.ToTable("exams");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasMaxLength(50);

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .HasConversion(title => title.Value, value => new Title(value));

        builder.Property(e => e.Price)
            .HasColumnType("decimal(18,2)")
            .HasConversion(price => price.Amount, value => new Money(value));

        builder.Property(e => e.Status)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<ExamStatus>(value)
            );

        builder.Property(e => e.InstructorId)
            .HasMaxLength(50);

        builder.Property(e => e.SubjectId)
            .HasMaxLength(50);

        builder.Property(e => e.ExamType)
            .HasConversion(
                type => type.ToString(),
                value => Enum.Parse<ExamType>(value)
            );

        builder.Property(e => e.ResultDisplay)
            .HasConversion(
                result => result.ToString(),
                value => Enum.Parse<ExamResultType>(value)
            );

        // Relationships
        builder.HasMany(e => e.Questions)
            .WithOne();

        builder.HasOne<Instructor>()
            .WithMany()
            .HasForeignKey(e => e.InstructorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(e => e.SubjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.InstructorId);
        builder.HasIndex(e => e.SubjectId);
    }
}
