using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeerReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class ClassesArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "term",
                table: "Classes",
                newName: "Term");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Classes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "Term",
                table: "Classes",
                newName: "term");
        }
    }
}
