using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infastructure.Migrations;

/// <inheritdoc />
public partial class AddLearningClasses : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_instructors_subjects_subject_id",
            table: "instructors");

        migrationBuilder.DropIndex(
            name: "ix_instructors_subject_id",
            table: "instructors");

        migrationBuilder.DropColumn(
            name: "subject_id",
            table: "instructors");

        migrationBuilder.AddColumn<string>(
            name: "class_id",
            table: "students",
            type: "nvarchar(450)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "class_id",
            table: "sessions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateTable(
            name: "learning_class",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_learning_class", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "classes_subjects",
            columns: table => new
            {
                class_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                subject_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_classes_subjects", x => new { x.subject_id, x.class_id });
                table.ForeignKey(
                    name: "fk_classes_subjects_learning_class_class_id",
                    column: x => x.class_id,
                    principalTable: "learning_class",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_classes_subjects_subject_subject_id",
                    column: x => x.subject_id,
                    principalTable: "subjects",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "classes_subjects_instructors",
            columns: table => new
            {
                class_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                subject_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                instructor_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_classes_subjects_instructors", x => new { x.subject_id, x.instructor_id, x.class_id });
                table.ForeignKey(
                    name: "fk_classes_subjects_instructors_instructor_instructor_id",
                    column: x => x.instructor_id,
                    principalTable: "instructors",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_classes_subjects_instructors_learning_class_class_id",
                    column: x => x.class_id,
                    principalTable: "learning_class",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_classes_subjects_instructors_subject_subject_id",
                    column: x => x.subject_id,
                    principalTable: "subjects",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_students_class_id",
            table: "students",
            column: "class_id");

        migrationBuilder.CreateIndex(
            name: "ix_classes_subjects_class_id",
            table: "classes_subjects",
            column: "class_id");

        migrationBuilder.CreateIndex(
            name: "ix_classes_subjects_instructors_class_id",
            table: "classes_subjects_instructors",
            column: "class_id");

        migrationBuilder.CreateIndex(
            name: "ix_classes_subjects_instructors_instructor_id",
            table: "classes_subjects_instructors",
            column: "instructor_id");

        migrationBuilder.CreateIndex(
            name: "ix_learning_class_type",
            table: "learning_class",
            column: "type",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "fk_students_learning_class_class_id",
            table: "students",
            column: "class_id",
            principalTable: "learning_class",
            principalColumn: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_students_learning_class_class_id",
            table: "students");

        migrationBuilder.DropTable(
            name: "classes_subjects");

        migrationBuilder.DropTable(
            name: "classes_subjects_instructors");

        migrationBuilder.DropTable(
            name: "learning_class");

        migrationBuilder.DropIndex(
            name: "ix_students_class_id",
            table: "students");

        migrationBuilder.DropColumn(
            name: "class_id",
            table: "students");

        migrationBuilder.DropColumn(
            name: "class_id",
            table: "sessions");

        migrationBuilder.AddColumn<string>(
            name: "subject_id",
            table: "instructors",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "ix_instructors_subject_id",
            table: "instructors",
            column: "subject_id");

        migrationBuilder.AddForeignKey(
            name: "fk_instructors_subjects_subject_id",
            table: "instructors",
            column: "subject_id",
            principalTable: "subjects",
            principalColumn: "id");
    }
}
