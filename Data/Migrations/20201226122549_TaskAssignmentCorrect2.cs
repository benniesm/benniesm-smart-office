using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartOffice.Data.Migrations
{
    public partial class TaskAssignmentCorrect2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RunnerDoneNotes",
                table: "TaskAssignment",
                newName: "Started");

            migrationBuilder.RenameColumn(
                name: "OwnerDoneNotes",
                table: "TaskAssignment",
                newName: "ExecutedNotes");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "TaskAssignment",
                newName: "Executed");

            migrationBuilder.AlterColumn<string>(
                name: "RunnerId",
                table: "TaskAssignment",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "TaskAssignment",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Completed",
                table: "TaskAssignment",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompletionNotes",
                table: "TaskAssignment",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "TaskAssignment");

            migrationBuilder.DropColumn(
                name: "CompletionNotes",
                table: "TaskAssignment");

            migrationBuilder.RenameColumn(
                name: "Started",
                table: "TaskAssignment",
                newName: "RunnerDoneNotes");

            migrationBuilder.RenameColumn(
                name: "ExecutedNotes",
                table: "TaskAssignment",
                newName: "OwnerDoneNotes");

            migrationBuilder.RenameColumn(
                name: "Executed",
                table: "TaskAssignment",
                newName: "FilePath");

            migrationBuilder.AlterColumn<string>(
                name: "RunnerId",
                table: "TaskAssignment",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "TaskAssignment",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
