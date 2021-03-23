using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartOffice.Data.Migrations
{
    public partial class TaskReviewCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    TaskReportId = table.Column<string>(type: "TEXT", nullable: true),
                    Details = table.Column<string>(type: "TEXT", nullable: false),
                    TimeIn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TaskReportId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskReview_TaskReport_TaskReportId1",
                        column: x => x.TaskReportId1,
                        principalTable: "TaskReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskReview_TaskReportId1",
                table: "TaskReview",
                column: "TaskReportId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskReview");
        }
    }
}
