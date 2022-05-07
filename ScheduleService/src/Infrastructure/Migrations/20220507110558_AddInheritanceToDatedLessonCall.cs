using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddInheritanceToDatedLessonCall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatedLessonCalls_LessonCalls_LessonCallId",
                table: "DatedLessonCalls");

            migrationBuilder.DropIndex(
                name: "IX_DatedLessonCalls_LessonCallId",
                table: "DatedLessonCalls");

            migrationBuilder.DropColumn(
                name: "LessonCallId",
                table: "DatedLessonCalls");

            migrationBuilder.AddForeignKey(
                name: "FK_DatedLessonCalls_LessonCalls_Id",
                table: "DatedLessonCalls",
                column: "Id",
                principalTable: "LessonCalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatedLessonCalls_LessonCalls_Id",
                table: "DatedLessonCalls");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonCallId",
                table: "DatedLessonCalls",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DatedLessonCalls_LessonCallId",
                table: "DatedLessonCalls",
                column: "LessonCallId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DatedLessonCalls_LessonCalls_LessonCallId",
                table: "DatedLessonCalls",
                column: "LessonCallId",
                principalTable: "LessonCalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
