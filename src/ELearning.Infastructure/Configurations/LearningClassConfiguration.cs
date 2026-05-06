using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Classes;
using ELearning.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
internal sealed class LearningClassConfiguration : IEntityTypeConfiguration<LearningClass>
{
    public void Configure(EntityTypeBuilder<LearningClass> builder)
    {
        builder.ToTable("learning_class");

        builder.Property(c => c.Type)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
            type => type.Value, value => new TypeName(value));

        builder.HasIndex(c => c.Type)
            .IsUnique();
    }
}
