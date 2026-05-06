using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Classes;
using ELearning.Domain.Instructors;
using ELearning.Domain.Subjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
internal sealed class ClassesSubjectConfiguration : IEntityTypeConfiguration<ClassesSubject>
{
    public void Configure(EntityTypeBuilder<ClassesSubject> builder)
    {
        builder.ToTable("classes_subjects");

        builder.HasKey(c => new { c.SubjectId, c.ClassId });

        builder.Property(c => c.SubjectId)
            .IsRequired()
            .HasMaxLength(50);

        // Relationships
        builder.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(c => c.SubjectId);

        builder.HasOne<LearningClass>()
            .WithMany()
            .HasForeignKey(c => c.ClassId);
    }
}
