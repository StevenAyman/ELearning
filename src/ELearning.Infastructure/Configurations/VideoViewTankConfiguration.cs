using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Enrollments;
using ELearning.Domain.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class VideoViewTankConfiguration : IEntityTypeConfiguration<VideoViewTank>
{
    public void Configure(EntityTypeBuilder<VideoViewTank> builder)
    {
        builder.ToTable("videos_view_tanks");
        builder.HasKey(vt => vt.Id);
        builder.Property(vt => vt.Id)
            .HasMaxLength(50);

        builder.Property(vt => vt.EnrollmentId)
            .HasMaxLength(50);

        builder.Property(vt => vt.VideoId)
            .HasMaxLength(50);

        builder.Property(vt => vt.MaxViewCount)
            .IsRequired(false);

        // Relationships

        builder.HasOne<Video>()
            .WithMany()
            .HasForeignKey(vt => vt.VideoId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        //builder.HasOne<Enrollment>()
        //    .WithMany()
        //    .HasForeignKey(vt => vt.EnrollmentId)
        //    .IsRequired()
        //    .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(vt => new { vt.EnrollmentId, vt.VideoId })
            .IsUnique();
    }
}
