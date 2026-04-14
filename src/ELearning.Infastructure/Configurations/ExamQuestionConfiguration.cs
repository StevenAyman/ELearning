using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;
using ELearning.Domain.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder.ToTable("exam_questions");
        builder.HasKey(eq => eq.Id);

        builder.Property(eq => eq.Id)
            .UseIdentityColumn();

        builder.Property(eq => eq.Title)
            .HasMaxLength(500)
            .HasConversion(title => title.Value, value => new Title(value));

        builder.Property(eq => eq.Question)
            .HasMaxLength(500);

        builder.Property(eq => eq.Type)
            .HasConversion(
                type => type.ToString(),
                type => Enum.Parse<ExamQuestionType>(type)
            );

        // Relationships

        builder.HasOne(eq => eq.CorrectAnswer)
            .WithOne()
            .HasForeignKey<ExamQuestion>("correct_answer_id")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(eq => eq.McqQuestionAnswers)
            .WithOne()
            .OnDelete(DeleteBehavior.SetNull);





    }
}
