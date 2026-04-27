using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infastructure.Migrations;

/// <inheritdoc />
public partial class UpdateRowVersionColumnAfterRemoving : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<byte[]>(
            name: "row_version",
            table: "students",
            type: "rowversion",
            rowVersion: true,
            nullable: true);

        migrationBuilder.AddColumn<byte[]>(
            name: "row_version",
            table: "sessions",
            type: "rowversion",
            rowVersion: true,
            nullable: true);

        migrationBuilder.AddColumn<byte[]>(
            name: "row_version",
            table: "instructors",
            type: "rowversion",
            rowVersion: true,
            nullable: true);

        migrationBuilder.AddColumn<byte[]>(
            name: "row_version",
            table: "discount_codes",
            type: "rowversion",
            rowVersion: true,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "row_version",
            table: "students");

        migrationBuilder.DropColumn(
            name: "row_version",
            table: "sessions");

        migrationBuilder.DropColumn(
            name: "row_version",
            table: "instructors");

        migrationBuilder.DropColumn(
            name: "row_version",
            table: "discount_codes");
    }
}
