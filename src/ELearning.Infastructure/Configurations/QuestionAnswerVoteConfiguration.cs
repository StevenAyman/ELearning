using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class QuestionAnswerVoteConfiguration : IEntityTypeConfiguration<QuestionAnswerVote>
{
    public void Configure(EntityTypeBuilder<QuestionAnswerVote> builder)
    {
        builder.ToTable("questions_votes");
        builder.Property(v => v.QuestionId)
            .HasMaxLength(50);

        builder.Property(v => v.StudentId)
            .HasMaxLength(50);

        builder.Property(v => v.VideoId)
            .HasMaxLength(50);

        builder.Property(v => v.VoteType)
            .IsRequired()   
            .HasConversion(
                vote => vote.ToString(),
                value => Enum.Parse<QuestionVoteType>(value)
            );

        // Relationships
        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(v => v.StudentId)
            .IsRequired();

        builder.HasOne<Video>()
            .WithMany()
            .HasForeignKey(v => v.VideoId)
            .IsRequired();

        builder.HasOne<VideoQuestion>()
            .WithMany()
            .HasForeignKey(v => v.QuestionId)
            .IsRequired();

        builder.HasIndex(v => v.Id)
            .IsUnique();

        builder.HasKey(v => new {v.StudentId, v.VideoId, v.QuestionId});
    }
}
