using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Ratings;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class SessionsRatingConfiguration : IEntityTypeConfiguration<SessionsRating>
{
    public void Configure(EntityTypeBuilder<SessionsRating> builder)
    {
        builder.ToTable("sessions_ratings");
        builder.HasKey(sr => new { sr.SessionId, sr.StudentId });

        builder.Property(sr => sr.SessionId)
            .HasMaxLength(50);

        builder.Property(sr => sr.StudentId)
            .HasMaxLength(50);

        builder.Property(sr => sr.Rating)
            .IsRequired()
            .HasConversion(rating => rating.Value, value => Rating.CreateRating(value));

        // Relationships
        builder.HasOne<Session>()
            .WithMany()
            .HasForeignKey(sr => sr.SessionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(sr => sr.StudentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
