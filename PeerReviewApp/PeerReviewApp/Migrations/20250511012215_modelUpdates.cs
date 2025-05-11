using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeerReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class modelUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Assignments_AssignmentId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ReviewGroups_ReviewGroupId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AssignmentId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewGroupId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewGroupId",
                table: "Reviews");

            migrationBuilder.CreateTable(
                name: "AssignmentSubmission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssignmentLink = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssignmentVersionId = table.Column<int>(type: "int", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    SubmitterId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmission_AspNetUsers_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssignmentSubmission_AssignmentVersions_AssignmentVersionId",
                        column: x => x.AssignmentVersionId,
                        principalTable: "AssignmentVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmission_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmission_AssignmentVersionId",
                table: "AssignmentSubmission",
                column: "AssignmentVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmission_ReviewId",
                table: "AssignmentSubmission",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmission_SubmitterId",
                table: "AssignmentSubmission",
                column: "SubmitterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentSubmission");

            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewGroupId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AssignmentId",
                table: "Reviews",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewGroupId",
                table: "Reviews",
                column: "ReviewGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Assignments_AssignmentId",
                table: "Reviews",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ReviewGroups_ReviewGroupId",
                table: "Reviews",
                column: "ReviewGroupId",
                principalTable: "ReviewGroups",
                principalColumn: "Id");
        }
    }
}
