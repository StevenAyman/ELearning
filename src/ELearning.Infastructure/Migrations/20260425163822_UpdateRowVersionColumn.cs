using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infastructure.Migrations;

/// <inheritdoc />
public partial class UpdateRowVersionColumn : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<long>(
            name: "row_version",
            table: "students",
            type: "bigint",
            rowVersion: true,
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.AddColumn<long>(
            name: "row_version",
            table: "sessions",
            type: "bigint",
            rowVersion: true,
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.AddColumn<long>(
            name: "row_version",
            table: "instructors",
            type: "bigint",
            rowVersion: true,
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.AddColumn<long>(
            name: "row_version",
            table: "discount_codes",
            type: "bigint",
            rowVersion: true,
            nullable: false,
            defaultValue: 0L);
    }
}
