using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class SessionQuizConfiguration : IEntityTypeConfiguration<SessionQuiz>
{
    public void Configure(EntityTypeBuilder<SessionQuiz> builder)
    {
        builder.ToTable("sessions_quizes");
        builder.HasKey(sq => sq.Id);
        builder.Property(sq => sq.Id)
            .HasMaxLength(50);

        builder.Property(sq => sq.InstructorId)
            .HasMaxLength(50);

        builder.Property(sq => sq.SubjectId)
            .HasMaxLength(50);

        builder.Property(sq => sq.Title)
            .HasMaxLength(200)
            .HasConversion(title => title.Value, value => new Title(value));

        builder.Property(sq => sq.PassingPercentage)
            .HasColumnType("decimal(18,2)")
            .HasConversion(perc => perc.Value, value => Percentage.Create(value));

        builder.Property(sq => sq.Status)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<QuizStatus>(value)
            );

        // Relationships

        builder.HasMany(sq => sq.Questions)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Instructor>()
            .WithMany()
            .HasForeignKey(sq => sq.InstructorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(sq => sq.SubjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(sq => sq.SubjectId);
        builder.HasIndex(sq => sq.InstructorId);
    }
}
