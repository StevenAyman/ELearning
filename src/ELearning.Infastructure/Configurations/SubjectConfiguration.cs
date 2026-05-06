using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Instructors;
using ELearning.Domain.Shared;
using ELearning.Domain.Subjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("subjects");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasMaxLength(50);

        builder.Property(s => s.Name)
            .HasMaxLength(50)
            .HasConversion(name => name.Value, value => new TypeName(value));

    }
}
