using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Reviews;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class InstructorReviewConfiguration : IEntityTypeConfiguration<InstructorsReview>
{
    public void Configure(EntityTypeBuilder<InstructorsReview> builder)
    {
        builder.ToTable("instructor_reviews");
        builder.HasKey(ir => new { ir.InstructorId, ir.StudentId });

        builder.Property(ir => ir.StudentId)
            .HasMaxLength(50);

        builder.Property(ir => ir.InstructorId)
            .HasMaxLength(50);

        builder.Property(ir => ir.Review)
            .HasMaxLength(500)
            .HasConversion(review => review.Value, value => new Review(value));

        // Relationships
        builder.HasOne<Instructor>()
            .WithMany()
            .HasForeignKey(ir => ir.InstructorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Student>()
            .WithMany()
            .HasForeignKey(ir => ir.StudentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
