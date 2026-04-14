using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "code_applicable_areas",
            columns: table => new
            {
                id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_code_applicable_areas", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "discount_codes",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                discount_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                expire_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                discount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                count_limit = table.Column<int>(type: "int", nullable: true),
                current_count = table.Column<int>(type: "int", nullable: false),
                expire_start_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                expire_end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                generated_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                expired_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                last_used_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                row_version = table.Column<long>(type: "bigint", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_discount_codes", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "outbox_messages",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                occurred_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                processed_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                error = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_outbox_messages", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "purchase_methods",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                type = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_purchase_methods", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "subjects",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_subjects", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                city = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                joined_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                identity_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "code_areas",
            columns: table => new
            {
                id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                appplicable_area_id = table.Column<int>(type: "int", nullable: false),
                code_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                target_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_code_areas", x => x.id);
                table.ForeignKey(
                    name: "fk_code_areas_code_applicable_areas_appplicable_area_id",
                    column: x => x.appplicable_area_id,
                    principalTable: "code_applicable_areas",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_code_areas_discount_code_code_id",
                    column: x => x.code_id,
                    principalTable: "discount_codes",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "instructors",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                rating_count = table.Column<int>(type: "int", nullable: false),
                rating_average = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                subject_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                row_version = table.Column<long>(type: "bigint", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_instructors", x => x.id);
                table.ForeignKey(
                    name: "fk_instructors_subjects_subject_id",
                    column: x => x.subject_id,
                    principalTable: "subjects",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_instructors_users_id",
                    column: x => x.id,
                    principalTable: "users",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "students",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                wallet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                row_version = table.Column<long>(type: "bigint", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_students", x => x.id);
                table.ForeignKey(
                    name: "fk_students_users_id",
                    column: x => x.id,
                    principalTable: "users",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "assistants",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                instructor_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_assistants", x => x.id);
                table.ForeignKey(
                    name: "fk_assistants_instructors_instructor_id",
                    column: x => x.instructor_id,
                    principalTable: "instructors",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_assistants_users_id",
                    column: x => x.id,
                    principalTable: "users",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "exams",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                questions_number = table.Column<int>(type: "int", nullable: false),
                total_grades = table.Column<int>(type: "int", nullable: false),
                price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                exam_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                instructor_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                subject_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                result_display = table.Column<string>(type: "nvarchar(max)", nullable: false),
                duration_in_seconds = table.Column<int>(type: "int", nullable: false),
                published_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_exams", x => x.id);
                table.ForeignKey(
                    name: "fk_exams_instructor_instructor_id",
                    column: x => x.instructor_id,
                    principalTable: "instructors",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_exams_subject_subject_id",
                    column: x => x.subject_id,
                    principalTable: "subjects",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "sessions_quizes",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                instructor_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                subject_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                total_grades = table.Column<int>(type: "int", nullable: false),
                total_questions = table.Column<int>(type: "int", nullable: false),
                duration_in_seconds = table.Column<int>(type: "int", nullable: false),
                passing_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                maximum_tries = table.Column<int>(type: "int", nullable: false),
                status = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sessions_quizes", x => x.id);
                table.ForeignKey(
                    name: "fk_sessions_quizes_instructors_instructor_id",
                    column: x => x.instructor_id,
                    principalTable: "instructors",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_sessions_quizes_subject_subject_id",
                    column: x => x.subject_id,
                    principalTable: "subjects",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "instructor_reviews",
            columns: table => new
            {
                instructor_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                student_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                review = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_instructor_reviews", x => new { x.instructor_id, x.student_id });
                table.ForeignKey(
                    name: "fk_instructor_reviews_instructors_instructor_id",
                    column: x => x.instructor_id,
                    principalTable: "instructors",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_instructor_reviews_student_student_id",
                    column: x => x.student_id,
                    principalTable: "students",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "instructors_ratings",
            columns: table => new
            {
                instructor_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                student_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                id = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_instructors_ratings", x => new { x.instructor_id, x.student_id });
                table.ForeignKey(
                    name: "fk_instructors_ratings_instructors_instructor_id",
                    column: x => x.instructor_id,
                    principalTable: "instructors",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_instructors_ratings_student_student_id",
                    column: x => x.student_id,
                    principalTable: "students",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "paid_codes",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                student_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                generated_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                used_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                expired_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                row_version = table.Column<long>(type: "bigint", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_paid_codes", x => x.id);
                table.ForeignKey(
                    name: "fk_paid_codes_student_student_id",
                    column: x => x.student_id,
                    principalTable: "students",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "sessions",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                instructor_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                subject_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                rating_count = table.Column<int>(type: "int", nullable: false),
                rating_average = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                published_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                last_updated_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                has_quiz = table.Column<bool>(type: "bit", nullable: false),
                row_version = table.Column<long>(type: "bigint", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sessions", x => x.id);
                table.ForeignKey(
                    name: "fk_sessions_instructors_instructor_id",
                    column: x => x.instructor_id,
                    principalTable: "instructors",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_sessions_session_quiz_id",
                    column: x => x.id,
                    principalTable: "sessions_quizes",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_sessions_subject_subject_id",
                    column: x => x.subject_id,
                    principalTable: "subjects",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "purchases",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                student_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                session_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                exam_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                payment_method_id = table.Column<string>(type: "nvarchar(50)", nullable: true),
                total_paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                code_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                purchased_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                refunded_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                completed_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_purchases", x => x.id);
                table.ForeignKey(
                    name: "fk_purchases_discount_codes_code_id",
                    column: x => x.code_id,
                    principalTable: "discount_codes",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "fk_purchases_exams_exam_id",
                    column: x => x.exam_id,
                    principalTable: "exams",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_purchases_purchase_method_payment_method_id",
                    column: x => x.payment_method_id,
                    principalTable: "purchase_methods",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "fk_purchases_session_session_id",
                    column: x => x.session_id,
                    principalTable: "sessions",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_purchases_student_student_id",
                    column: x => x.student_id,
                    principalTable: "students",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "sessions_ratings",
            columns: table => new
            {
                session_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                student_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                id = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sessions_ratings", x => new { x.session_id, x.student_id });
                table.ForeignKey(
                    name: "fk_sessions_ratings_sessions_session_id",
                    column: x => x.session_id,
                    principalTable: "sessions",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_sessions_ratings_student_student_id",
                    column: x => x.student_id,
                    principalTable: "students",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "videos",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                order = table.Column<int>(type: "int", nullable: false),
                duration_in_seconds = table.Column<int>(type: "int", nullable: false),
                threshold_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                max_view_count = table.Column<int>(type: "int", nullable: true),
                has_prerequisite = table.Column<bool>(type: "bit", nullable: false),
                prerequisite_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                is_counted_in_session_progression = table.Column<bool>(type: "bit", nullable: false),
                session_id = table.Column<string>(type: "nvarchar(50)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_videos", x => x.id);
                table.ForeignKey(
                    name: "fk_videos_sessions_session_id",
                    column: x => x.session_id,
                    principalTable: "sessions",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_videos_videos_prerequisite_id",
                    column: x => x.prerequisite_id,
                    principalTable: "videos",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "enrollments",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                student_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                session_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                purchase_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                quiz_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                max_tries = table.Column<int>(type: "int", nullable: true),
                remaining_tries = table.Column<int>(type: "int", nullable: true),
                status = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_enrollments", x => x.id);
                table.ForeignKey(
                    name: "fk_enrollments_purchase_purchase_id",
                    column: x => x.purchase_id,
                    principalTable: "purchases",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_enrollments_session_quiz_quiz_id",
                    column: x => x.quiz_id,
                    principalTable: "sessions_quizes",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "fk_enrollments_session_session_id",
                    column: x => x.session_id,
                    principalTable: "sessions",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_enrollments_student_student_id",
                    column: x => x.student_id,
                    principalTable: "students",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "exam_enrollments",
            columns: table => new
            {
                purchase_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                student_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                exam_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                duration_in_seconds = table.Column<int>(type: "int", nullable: false),
                enrolled_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                finished_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                grade = table.Column<double>(type: "float", nullable: true),
                total_grade = table.Column<int>(type: "int", nullable: false),
                id = table.Column<string>(type: "nvarchar(450)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_exam_enrollments", x => new { x.purchase_id, x.student_id, x.exam_id });
                table.ForeignKey(
                    name: "fk_exam_enrollments_exams_exam_id",
                    column: x => x.exam_id,
                    principalTable: "exams",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_exam_enrollments_purchase_purchase_id",
                    column: x => x.purchase_id,
                    principalTable: "purchases",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_exam_enrollments_student_student_id",
                    column: x => x.student_id,
                    principalTable: "students",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "videos_questions",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                student_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                video_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                session_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                assistant_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                vote = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_videos_questions", x => x.id);
                table.ForeignKey(
                    name: "fk_videos_questions_assistants_assistant_id",
                    column: x => x.assistant_id,
                    principalTable: "assistants",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "fk_videos_questions_sessions_session_id",
                    column: x => x.session_id,
                    principalTable: "sessions",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_videos_questions_students_student_id",
                    column: x => x.student_id,
                    principalTable: "students",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_videos_questions_videos_video_id",
                    column: x => x.video_id,
                    principalTable: "videos",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "users_quizes",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                enrollment_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                quiz_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                score = table.Column<double>(type: "float", nullable: true),
                total_score = table.Column<double>(type: "float", nullable: false),
                threshold = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                duration_in_seconds = table.Column<int>(type: "int", nullable: false),
                issued_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                finish_time_in_seconds = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users_quizes", x => x.id);
                table.ForeignKey(
                    name: "fk_users_quizes_enrollments_enrollment_id",
                    column: x => x.enrollment_id,
                    principalTable: "enrollments",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_users_quizes_sessions_quizes_quiz_id",
                    column: x => x.quiz_id,
                    principalTable: "sessions_quizes",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "videos_view_tanks",
            columns: table => new
            {
                id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                enrollment_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                video_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                max_view_count = table.Column<int>(type: "int", nullable: true),
                used_view_count = table.Column<int>(type: "int", nullable: false),
                duration_threshold = table.Column<int>(type: "int", nullable: false),
                video_duration_seconds = table.Column<int>(type: "int", nullable: false),
                tank_seconds_capacity = table.Column<int>(type: "int", nullable: false),
                remaining_tank_seconds = table.Column<int>(type: "int", nullable: false),
                is_unlocked = table.Column<bool>(type: "bit", nullable: false),
                is_completed = table.Column<bool>(type: "bit", nullable: false),
                last_position_in_seconds = table.Column<int>(type: "int", nullable: false),
                best_position_in_seconds = table.Column<int>(type: "int", nullable: false),
                last_active_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_videos_view_tanks", x => x.id);
                table.ForeignKey(
                    name: "fk_videos_view_tanks_enrollments_enrollment_id",
                    column: x => x.enrollment_id,
                    principalTable: "enrollments",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_videos_view_tanks_videos_video_id",
                    column: x => x.video_id,
                    principalTable: "videos",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "questions_votes",
            columns: table => new
            {
                video_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                question_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                student_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                vote_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                id = table.Column<string>(type: "nvarchar(450)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_questions_votes", x => new { x.student_id, x.video_id, x.question_id });
                table.ForeignKey(
                    name: "fk_questions_votes_student_student_id",
                    column: x => x.student_id,
                    principalTable: "students",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_questions_votes_video_question_question_id",
                    column: x => x.question_id,
                    principalTable: "videos_questions",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_questions_votes_video_video_id",
                    column: x => x.video_id,
                    principalTable: "videos",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "exam_mcq_questions_answers",
            columns: table => new
            {
                answer_id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                answer = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                exam_question_id = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_exam_mcq_questions_answers", x => x.answer_id);
            });

        migrationBuilder.CreateTable(
            name: "exam_questions",
            columns: table => new
            {
                id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                correct_answer_id = table.Column<int>(type: "int", nullable: true),
                grade = table.Column<double>(type: "float", nullable: false),
                exam_id = table.Column<string>(type: "nvarchar(50)", nullable: true),
                session_quiz_id = table.Column<string>(type: "nvarchar(50)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_exam_questions", x => x.id);
                table.ForeignKey(
                    name: "fk_exam_questions_exam_mcq_questions_answers_correct_answer_id",
                    column: x => x.correct_answer_id,
                    principalTable: "exam_mcq_questions_answers",
                    principalColumn: "answer_id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "fk_exam_questions_exams_exam_id",
                    column: x => x.exam_id,
                    principalTable: "exams",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_exam_questions_session_quiz_session_quiz_id",
                    column: x => x.session_quiz_id,
                    principalTable: "sessions_quizes",
                    principalColumn: "id");
            });

        migrationBuilder.CreateTable(
            name: "exam_questions_answers",
            columns: table => new
            {
                user_quiz_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                question_id = table.Column<int>(type: "int", nullable: false),
                answer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_exam_questions_answers", x => new { x.user_quiz_id, x.question_id });
                table.ForeignKey(
                    name: "fk_exam_questions_answers_exam_question_question_id",
                    column: x => x.question_id,
                    principalTable: "exam_questions",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_exam_questions_answers_user_quiz_user_quiz_id",
                    column: x => x.user_quiz_id,
                    principalTable: "users_quizes",
                    principalColumn: "id");
            });

        migrationBuilder.CreateIndex(
            name: "ix_assistants_instructor_id",
            table: "assistants",
            column: "instructor_id");

        migrationBuilder.CreateIndex(
            name: "ix_code_areas_appplicable_area_id_code_id",
            table: "code_areas",
            columns: [ "appplicable_area_id", "code_id" ]);

        migrationBuilder.CreateIndex(
            name: "ix_code_areas_code_id",
            table: "code_areas",
            column: "code_id");

        migrationBuilder.CreateIndex(
            name: "ix_discount_codes_code",
            table: "discount_codes",
            column: "code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_enrollments_purchase_id",
            table: "enrollments",
            column: "purchase_id",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_enrollments_quiz_id",
            table: "enrollments",
            column: "quiz_id");

        migrationBuilder.CreateIndex(
            name: "ix_enrollments_session_id",
            table: "enrollments",
            column: "session_id");

        migrationBuilder.CreateIndex(
            name: "ix_enrollments_student_id",
            table: "enrollments",
            column: "student_id");

        migrationBuilder.CreateIndex(
            name: "ix_exam_enrollments_exam_id",
            table: "exam_enrollments",
            column: "exam_id");

        migrationBuilder.CreateIndex(
            name: "ix_exam_enrollments_id",
            table: "exam_enrollments",
            column: "id",
            unique: true,
            filter: "[id] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "ix_exam_enrollments_purchase_id",
            table: "exam_enrollments",
            column: "purchase_id",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_exam_enrollments_student_id",
            table: "exam_enrollments",
            column: "student_id");

        migrationBuilder.CreateIndex(
            name: "ix_exam_mcq_questions_answers_exam_question_id",
            table: "exam_mcq_questions_answers",
            column: "exam_question_id");

        migrationBuilder.CreateIndex(
            name: "ix_exam_questions_correct_answer_id",
            table: "exam_questions",
            column: "correct_answer_id",
            unique: true,
            filter: "[correct_answer_id] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "ix_exam_questions_exam_id",
            table: "exam_questions",
            column: "exam_id");

        migrationBuilder.CreateIndex(
            name: "ix_exam_questions_session_quiz_id",
            table: "exam_questions",
            column: "session_quiz_id");

        migrationBuilder.CreateIndex(
            name: "ix_exam_questions_answers_question_id",
            table: "exam_questions_answers",
            column: "question_id");

        migrationBuilder.CreateIndex(
            name: "ix_exams_instructor_id",
            table: "exams",
            column: "instructor_id");

        migrationBuilder.CreateIndex(
            name: "ix_exams_subject_id",
            table: "exams",
            column: "subject_id");

        migrationBuilder.CreateIndex(
            name: "ix_instructor_reviews_student_id",
            table: "instructor_reviews",
            column: "student_id");

        migrationBuilder.CreateIndex(
            name: "ix_instructors_subject_id",
            table: "instructors",
            column: "subject_id");

        migrationBuilder.CreateIndex(
            name: "ix_instructors_ratings_student_id",
            table: "instructors_ratings",
            column: "student_id");

        migrationBuilder.CreateIndex(
            name: "ix_paid_codes_code",
            table: "paid_codes",
            column: "code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_paid_codes_student_id",
            table: "paid_codes",
            column: "student_id");

        migrationBuilder.CreateIndex(
            name: "ix_purchases_code_id",
            table: "purchases",
            column: "code_id");

        migrationBuilder.CreateIndex(
            name: "ix_purchases_exam_id",
            table: "purchases",
            column: "exam_id");

        migrationBuilder.CreateIndex(
            name: "ix_purchases_payment_method_id",
            table: "purchases",
            column: "payment_method_id");

        migrationBuilder.CreateIndex(
            name: "ix_purchases_session_id",
            table: "purchases",
            column: "session_id");

        migrationBuilder.CreateIndex(
            name: "ix_purchases_student_id_exam_id",
            table: "purchases",
            columns: [ "student_id", "exam_id" ]);

        migrationBuilder.CreateIndex(
            name: "ix_purchases_student_id_session_id",
            table: "purchases",
            columns: [ "student_id", "session_id" ]);

        migrationBuilder.CreateIndex(
            name: "ix_questions_votes_id",
            table: "questions_votes",
            column: "id",
            unique: true,
            filter: "[id] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "ix_questions_votes_question_id",
            table: "questions_votes",
            column: "question_id");

        migrationBuilder.CreateIndex(
            name: "ix_questions_votes_video_id",
            table: "questions_votes",
            column: "video_id");

        migrationBuilder.CreateIndex(
            name: "ix_sessions_instructor_id",
            table: "sessions",
            column: "instructor_id");

        migrationBuilder.CreateIndex(
            name: "ix_sessions_subject_id",
            table: "sessions",
            column: "subject_id");

        migrationBuilder.CreateIndex(
            name: "ix_sessions_quizes_instructor_id",
            table: "sessions_quizes",
            column: "instructor_id");

        migrationBuilder.CreateIndex(
            name: "ix_sessions_quizes_subject_id",
            table: "sessions_quizes",
            column: "subject_id");

        migrationBuilder.CreateIndex(
            name: "ix_sessions_ratings_student_id",
            table: "sessions_ratings",
            column: "student_id");

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            table: "users",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_users_identity_id",
            table: "users",
            column: "identity_id",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_users_quizes_enrollment_id_quiz_id",
            table: "users_quizes",
            columns: [ "enrollment_id", "quiz_id" ]);

        migrationBuilder.CreateIndex(
            name: "ix_users_quizes_quiz_id",
            table: "users_quizes",
            column: "quiz_id");

        migrationBuilder.CreateIndex(
            name: "ix_videos_prerequisite_id",
            table: "videos",
            column: "prerequisite_id",
            unique: true,
            filter: "[prerequisite_id] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "ix_videos_session_id",
            table: "videos",
            column: "session_id");

        migrationBuilder.CreateIndex(
            name: "ix_videos_questions_assistant_id",
            table: "videos_questions",
            column: "assistant_id");

        migrationBuilder.CreateIndex(
            name: "ix_videos_questions_session_id",
            table: "videos_questions",
            column: "session_id");

        migrationBuilder.CreateIndex(
            name: "ix_videos_questions_student_id",
            table: "videos_questions",
            column: "student_id");

        migrationBuilder.CreateIndex(
            name: "ix_videos_questions_video_id",
            table: "videos_questions",
            column: "video_id");

        migrationBuilder.CreateIndex(
            name: "ix_videos_view_tanks_enrollment_id_video_id",
            table: "videos_view_tanks",
            columns: [ "enrollment_id", "video_id" ],
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_videos_view_tanks_video_id",
            table: "videos_view_tanks",
            column: "video_id");

        migrationBuilder.AddForeignKey(
            name: "fk_exam_mcq_questions_answers_exam_question_exam_question_id",
            table: "exam_mcq_questions_answers",
            column: "exam_question_id",
            principalTable: "exam_questions",
            principalColumn: "id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_exams_instructor_instructor_id",
            table: "exams");

        migrationBuilder.DropForeignKey(
            name: "fk_sessions_quizes_instructors_instructor_id",
            table: "sessions_quizes");

        migrationBuilder.DropForeignKey(
            name: "fk_exam_questions_session_quiz_session_quiz_id",
            table: "exam_questions");

        migrationBuilder.DropForeignKey(
            name: "fk_exam_questions_exams_exam_id",
            table: "exam_questions");

        migrationBuilder.DropForeignKey(
            name: "fk_exam_mcq_questions_answers_exam_question_exam_question_id",
            table: "exam_mcq_questions_answers");

        migrationBuilder.DropTable(
            name: "code_areas");

        migrationBuilder.DropTable(
            name: "exam_enrollments");

        migrationBuilder.DropTable(
            name: "exam_questions_answers");

        migrationBuilder.DropTable(
            name: "instructor_reviews");

        migrationBuilder.DropTable(
            name: "instructors_ratings");

        migrationBuilder.DropTable(
            name: "outbox_messages");

        migrationBuilder.DropTable(
            name: "paid_codes");

        migrationBuilder.DropTable(
            name: "questions_votes");

        migrationBuilder.DropTable(
            name: "sessions_ratings");

        migrationBuilder.DropTable(
            name: "videos_view_tanks");

        migrationBuilder.DropTable(
            name: "code_applicable_areas");

        migrationBuilder.DropTable(
            name: "users_quizes");

        migrationBuilder.DropTable(
            name: "videos_questions");

        migrationBuilder.DropTable(
            name: "enrollments");

        migrationBuilder.DropTable(
            name: "assistants");

        migrationBuilder.DropTable(
            name: "videos");

        migrationBuilder.DropTable(
            name: "purchases");

        migrationBuilder.DropTable(
            name: "discount_codes");

        migrationBuilder.DropTable(
            name: "purchase_methods");

        migrationBuilder.DropTable(
            name: "sessions");

        migrationBuilder.DropTable(
            name: "students");

        migrationBuilder.DropTable(
            name: "instructors");

        migrationBuilder.DropTable(
            name: "users");

        migrationBuilder.DropTable(
            name: "sessions_quizes");

        migrationBuilder.DropTable(
            name: "exams");

        migrationBuilder.DropTable(
            name: "subjects");

        migrationBuilder.DropTable(
            name: "exam_questions");

        migrationBuilder.DropTable(
            name: "exam_mcq_questions_answers");
    }
}
