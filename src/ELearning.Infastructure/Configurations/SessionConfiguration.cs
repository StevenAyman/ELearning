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
public sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasMaxLength(50);

        builder.Property(s => s.Title)
            .HasMaxLength(200)
            .HasConversion(title => title.Value, value => new Title(value));

        builder.Property(s => s.Description)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasConversion(
            desc => desc != null? desc.Value : null, 
            value => value != null? new Description(value) : null);

        builder.Property(s => s.Price)
            .HasColumnType("decimal(18,2)")
            .HasConversion(price => price.Amount, value => new Money(value));

        builder.Property(s => s.InstructorId)
            .HasMaxLength(50);

        builder.Property(s => s.SubjectId)
            .HasMaxLength(50);

        builder.Property(s => s.Status)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<SessionStatus>(value)
            );

        builder.OwnsOne(s => s.Rating, ratingBuilder =>
        {
            ratingBuilder.Property(r => r.Count)
            .HasColumnName("rating_count");

            ratingBuilder.Property(r => r.Average)
            .HasColumnType("decimal(18,2)")
            .HasConversion(average => average.Value, value => Rating.CreateRating(value))
            .HasColumnName("rating_average");
        });

        builder.Property<uint>("row_version")
            .IsRowVersion();

        // Relationships

        builder.HasMany(s => s.Videos)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Instructor>()
            .WithMany()
            .HasForeignKey(s => s.InstructorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(s => s.SubjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(s => s.Quiz)
            .WithOne()
            .HasForeignKey<Session>()
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(s => s.InstructorId);
        builder.HasIndex(s => s.SubjectId);
    }
}
