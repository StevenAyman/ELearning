using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Enrollments;
using ELearning.Domain.Purchases;
using ELearning.Domain.Sessions;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("enrollments");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasMaxLength(50);

        builder.Property(e => e.StudentId)
            .HasMaxLength(50);

        builder.Property(e => e.SessionId)
            .HasMaxLength(50);

        builder.Property(e => e.PurchaseId)
            .HasMaxLength(50);

        builder.Property(e => e.QuizId)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(e => e.MaxTries)
            .IsRequired(false);

        builder.Property(e => e.RemainingTries)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .HasConversion(
                s => s.ToString(),
                s => Enum.Parse<EnrollmentStatus>(s)
            );

        // Relationships

        builder.HasOne<SessionQuiz>()
            .WithMany()
            .HasForeignKey(e => e.QuizId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(e => e.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(e => e.StudentId);

        builder.HasOne<Purchase>()
            .WithOne()
            .HasForeignKey<Enrollment>(e => e.PurchaseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(e => e.PurchaseId)
            .IsUnique();

        builder.HasOne<Session>()
            .WithMany()
            .HasForeignKey(e => e.SessionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(e => e.SessionId);

        builder.HasMany(e => e.Tanks)
            .WithOne();
    }
}
