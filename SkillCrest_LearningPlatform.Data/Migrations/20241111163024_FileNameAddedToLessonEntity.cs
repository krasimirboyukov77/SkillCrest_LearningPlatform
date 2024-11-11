using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCrest_LearningPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class FileNameAddedToLessonEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Lessons");
        }
    }
}
