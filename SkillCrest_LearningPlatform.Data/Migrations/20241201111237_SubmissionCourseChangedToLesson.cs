using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCrest_LearningPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class SubmissionCourseChangedToLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Courses_CourseId",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Submissions",
                newName: "LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_CourseId",
                table: "Submissions",
                newName: "IX_Submissions_LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Courses_LessonId",
                table: "Submissions",
                column: "LessonId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Courses_LessonId",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "Submissions",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_LessonId",
                table: "Submissions",
                newName: "IX_Submissions_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Courses_CourseId",
                table: "Submissions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
