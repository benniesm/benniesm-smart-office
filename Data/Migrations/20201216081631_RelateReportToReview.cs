using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartOffice.Data.Migrations
{
    public partial class RelateReportToReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskReview_TaskReport_TaskReportId1",
                table: "TaskReview");

            migrationBuilder.DropIndex(
                name: "IX_TaskReview_TaskReportId1",
                table: "TaskReview");

            migrationBuilder.DropColumn(
                name: "TaskReportId1",
                table: "TaskReview");

            migrationBuilder.AlterColumn<int>(
                name: "TaskReportId",
                table: "TaskReview",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskReview_TaskReportId",
                table: "TaskReview",
                column: "TaskReportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskReview_TaskReport_TaskReportId",
                table: "TaskReview",
                column: "TaskReportId",
                principalTable: "TaskReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskReview_TaskReport_TaskReportId",
                table: "TaskReview");

            migrationBuilder.DropIndex(
                name: "IX_TaskReview_TaskReportId",
                table: "TaskReview");

            migrationBuilder.AlterColumn<string>(
                name: "TaskReportId",
                table: "TaskReview",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "TaskReportId1",
                table: "TaskReview",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskReview_TaskReportId1",
                table: "TaskReview",
                column: "TaskReportId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskReview_TaskReport_TaskReportId1",
                table: "TaskReview",
                column: "TaskReportId1",
                principalTable: "TaskReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
