using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCrest_LearningPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedSubmissionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Courses_LessonId",
                table: "Submissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Lessons_LessonId",
                table: "Submissions",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Lessons_LessonId",
                table: "Submissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Courses_LessonId",
                table: "Submissions",
                column: "LessonId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
