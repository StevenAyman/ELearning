using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Enrollments;
using ELearning.Domain.Exams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class ExamQuestionAnswerConfiguration : IEntityTypeConfiguration<ExamQuestionAnswer>
{
    public void Configure(EntityTypeBuilder<ExamQuestionAnswer> builder)
    {
        builder.ToTable("exam_questions_answers");
        builder.HasKey(a => new {a.UserQuizId, a.QuestionId});
        builder.Property(a => a.Answer)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(a => a.UserQuizId)
            .HasMaxLength(50);

        // Relationships
        builder.HasOne<ExamQuestion>()
            .WithMany()
            .HasForeignKey(a => a.QuestionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<UserQuiz>()
            .WithMany()
            .HasForeignKey(a => a.UserQuizId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
