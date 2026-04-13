using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Assistants;
using ELearning.Domain.Instructors;
using ELearning.Domain.Students;
using ELearning.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELearning.Infastructure.Configurations;
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasMaxLength(50);

        builder.Property(u => u.FirstName)
            .HasMaxLength(50)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value));

        builder.Property(u => u.LastName)
            .HasMaxLength(50)
            .HasConversion(lastName => lastName.Value, value => new LastName(value));

        builder.Property(u => u.Email)
            .HasMaxLength(50)
            .HasConversion(u => u.Value, value => new Email(value));

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.City)
            .HasMaxLength(30);

        builder.Property(u => u.DateOfBirth)
            .HasConversion(dob => dob.Value, value => Date.Create(value));

        builder.Property(u => u.IdentityId)
            .HasMaxLength(50);

        builder.HasIndex(u => u.IdentityId)
            .IsUnique();


        // Relatioships
        builder.HasOne<Student>()
            .WithOne()
            .HasForeignKey<Student>(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Instructor>()
            .WithOne()
            .HasForeignKey<Instructor>(i => i.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Assistant>()
            .WithOne()
            .HasForeignKey<Assistant>(a => a.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


    }
}
