using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Ratings;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class InstructorsRatingConfiguration : IEntityTypeConfiguration<InstructorsRating>
{
    public void Configure(EntityTypeBuilder<InstructorsRating> builder)
    {
        builder.ToTable("instructors_ratings");
        builder.HasKey(ir => new { ir.InstructorId, ir.StudentId });

        builder.Property(ir => ir.InstructorId)
            .HasMaxLength(50);

        builder.Property(ir => ir.StudentId)
            .HasMaxLength(50);

        builder.Property(ir => ir.Rating)
            .IsRequired()
            .HasConversion(rating => rating.Value, value => Rating.CreateRating(value));

        // Relationships
        builder.HasOne<Instructor>()
            .WithMany()
            .HasForeignKey(ir => ir.InstructorId);

        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(ir => ir.StudentId);

    }
}
