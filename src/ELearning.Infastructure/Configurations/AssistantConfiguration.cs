using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Assistants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class AssistantConfiguration : IEntityTypeConfiguration<Assistant>
{
    public void Configure(EntityTypeBuilder<Assistant> builder)
    {
        builder.ToTable("assistants");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasMaxLength(50);

        builder.Property(a => a.InstructorId)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(a => a.InstructorId);
    }
}
