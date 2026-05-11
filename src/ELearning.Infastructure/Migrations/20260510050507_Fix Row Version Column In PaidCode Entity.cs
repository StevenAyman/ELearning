using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infastructure.Migrations;

/// <inheritdoc />
public partial class FixRowVersionColumnInPaidCodeEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "row_version",
            table: "paid_codes");

        migrationBuilder.AddColumn<byte[]>(
            name: "row_version",
            table: "paid_codes",
            rowVersion: true,
            nullable: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "row_version",
            table: "paid_codes");

        migrationBuilder.AddColumn<uint>(
            name: "row_version",
            table: "paid_codes",
            rowVersion: true,
            nullable: false);
    }
}
