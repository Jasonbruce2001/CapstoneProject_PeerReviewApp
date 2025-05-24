using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeerReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class gradeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Assignments_AssignmentId",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Grades_AssignmentId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Grades");

            migrationBuilder.AddColumn<int>(
                name: "ReviewGradeId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignmentGradeId",
                table: "AssignmentSubmissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewGradeId",
                table: "Reviews",
                column: "ReviewGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_AssignmentGradeId",
                table: "AssignmentSubmissions",
                column: "AssignmentGradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentSubmissions_Grades_AssignmentGradeId",
                table: "AssignmentSubmissions",
                column: "AssignmentGradeId",
                principalTable: "Grades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Grades_ReviewGradeId",
                table: "Reviews",
                column: "ReviewGradeId",
                principalTable: "Grades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentSubmissions_Grades_AssignmentGradeId",
                table: "AssignmentSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Grades_ReviewGradeId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewGradeId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentSubmissions_AssignmentGradeId",
                table: "AssignmentSubmissions");

            migrationBuilder.DropColumn(
                name: "ReviewGradeId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AssignmentGradeId",
                table: "AssignmentSubmissions");

            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Grades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_AssignmentId",
                table: "Grades",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Assignments_AssignmentId",
                table: "Grades",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
