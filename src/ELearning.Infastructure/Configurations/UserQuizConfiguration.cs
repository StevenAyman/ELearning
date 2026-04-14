using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Enrollments;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class UserQuizConfiguration : IEntityTypeConfiguration<UserQuiz>
{
    public void Configure(EntityTypeBuilder<UserQuiz> builder)
    {
        builder.ToTable("users_quizes");
        builder.HasKey(uq => uq.Id);

        builder.Property(uq => uq.Id)
            .HasMaxLength(50);

        builder.Property(uq => uq.EnrollmentId)
            .HasMaxLength(50);

        builder.Property(uq => uq.QuizId)
            .HasMaxLength(50);

        builder.Property(uq => uq.Score)
            .IsRequired(false);

        builder.Property(uq => uq.TotalScore)
            .IsRequired();

        builder.Property(uq => uq.Threshold)
            .HasColumnType("decimal(18,2)")
            .HasConversion(percentage => percentage.Value, value => Percentage.Create(value));


        // Relationships
        builder.HasOne<Enrollment>()
            .WithMany()
            .HasForeignKey(uq => uq.EnrollmentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<SessionQuiz>()
            .WithMany()
            .HasForeignKey(uq => uq.QuizId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(uq => new {uq.EnrollmentId, uq.QuizId});
    }
}
