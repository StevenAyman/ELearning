using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Sessions;
using ELearning.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("videos");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasMaxLength(50);

        builder.Property(v => v.Title)
            .HasMaxLength(200)
            .HasConversion(title => title.Value, value => new Title(value));

        builder.Property(v => v.Url)
            .HasMaxLength(1000);

        builder.Property(v => v.Order)
            .HasConversion(order => order.Value, value => VideoOrder.Create(value));

        builder.Property(v => v.ThresholdPercentage)
            .HasColumnType("deicmal(18,2)")
            .HasConversion(percentage => percentage.Value, value => Percentage.Create(value));

        builder.Property(v => v.MaxViewCount)
            .IsRequired(false);

        builder.Property(v => v.PrerequisiteId)
            .IsRequired(false)
            .HasMaxLength(50);

        // Relationships
        builder.HasOne<Video>()
            .WithOne()
            .HasForeignKey<Video>(v => v.PrerequisiteId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(v => v.PrerequisiteId);
    }
}
