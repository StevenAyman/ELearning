using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Exams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class ExamMcqQuestionAnswerConfiguration : IEntityTypeConfiguration<ExamMcqQuestionAnswer>
{
    public void Configure(EntityTypeBuilder<ExamMcqQuestionAnswer> builder)
    {
        builder.ToTable("exam_mcq_questions_answers");
        builder.HasKey(ea => ea.AnswerId);
        builder.Property(ea => ea.AnswerId)
            .UseIdentityColumn();

        builder.Property(ea => ea.Answer)
            .IsRequired()
            .HasMaxLength(500);


    }
}
