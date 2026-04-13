using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Assistants;
using ELearning.Domain.Sessions;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class VideoQuestionConfiguration : IEntityTypeConfiguration<VideoQuestion>
{
    public void Configure(EntityTypeBuilder<VideoQuestion> builder)
    {
        builder.ToTable("videos_questions");
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .HasMaxLength(50);

        builder.Property(q => q.StudentId)
            .HasMaxLength(50);

        builder.Property(q => q.VideoId)
            .HasMaxLength(50);

        builder.Property(q => q.SessionId)
            .HasMaxLength(50);

        builder.Property(q => q.AssistantId)
            .HasMaxLength(50);

        builder.Property(q => q.Status)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<QuestionStatus>(value)
            );

        // Relationships
        builder.HasOne<Video>()
            .WithMany()
            .HasForeignKey(q => q.VideoId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(q => q.VideoId);

        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(q => q.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(q => q.StudentId);

        builder.HasOne<Session>()
            .WithMany()
            .HasForeignKey(q => q.SessionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(q => q.SessionId);

        builder.HasOne<Assistant>()
            .WithMany()
            .HasForeignKey(q => q.AssistantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

    }
}
