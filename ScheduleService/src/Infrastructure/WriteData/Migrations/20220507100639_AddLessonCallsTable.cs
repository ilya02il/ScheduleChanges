using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddLessonCallsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangesLists_EducationalOrgs_EducationalOrgId",
                table: "ChangesLists");

            migrationBuilder.CreateTable(
                name: "LessonCalls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EducationalOrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonCalls_EducationalOrgs_EducationalOrgId",
                        column: x => x.EducationalOrgId,
                        principalTable: "EducationalOrgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DatedLessonCalls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LessonCallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatedLessonCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatedLessonCalls_LessonCalls_LessonCallId",
                        column: x => x.LessonCallId,
                        principalTable: "LessonCalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatedLessonCalls_LessonCallId",
                table: "DatedLessonCalls",
                column: "LessonCallId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonCalls_EducationalOrgId",
                table: "LessonCalls",
                column: "EducationalOrgId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangesLists_EducationalOrgs_EducationalOrgId",
                table: "ChangesLists",
                column: "EducationalOrgId",
                principalTable: "EducationalOrgs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangesLists_EducationalOrgs_EducationalOrgId",
                table: "ChangesLists");

            migrationBuilder.DropTable(
                name: "DatedLessonCalls");

            migrationBuilder.DropTable(
                name: "LessonCalls");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangesLists_EducationalOrgs_EducationalOrgId",
                table: "ChangesLists",
                column: "EducationalOrgId",
                principalTable: "EducationalOrgs",
                principalColumn: "Id");
        }
    }
}
