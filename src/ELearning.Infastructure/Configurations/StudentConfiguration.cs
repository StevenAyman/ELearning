using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;
using ELearning.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasMaxLength(50);

        builder.Property(s => s.Wallet)
            .HasColumnType("decimal(18,2)")
            .HasConversion(wallet => wallet.Amount, value => new Money(value));

        builder.Property<byte[]>("row_version")
            .IsRowVersion();


    }
}
