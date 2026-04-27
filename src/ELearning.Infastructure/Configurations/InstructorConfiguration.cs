using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Assistants;
using ELearning.Domain.Instructors;
using ELearning.Domain.Reviews;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.ToTable("instructors");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasMaxLength(50);

        builder.Property(i => i.Bio)
            .HasMaxLength(500)
            .HasConversion(bio => bio == null? null : bio.Value, value => new Bio(value));

        builder.OwnsOne(i => i.Rating, ratingBuilder =>
        {
            ratingBuilder.Property(r => r.Average)
                .HasColumnType("decimal(18,2)")
                .HasConversion(rating => rating.Value, value => Rating.CreateRating(value));

            ratingBuilder.Property(r => r.Count)
                .HasColumnName("rating_count");

            ratingBuilder.Property(r => r.Average)
                .HasColumnName("rating_average");
        });

        builder.Property<byte[]>("row_version")
            .IsRowVersion();

        builder.Property(i => i.SubjectId)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(i => i.SubjectId);

        // Relationships
        builder.HasMany<Assistant>()
            .WithOne()
            .HasForeignKey(a => a.InstructorId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
