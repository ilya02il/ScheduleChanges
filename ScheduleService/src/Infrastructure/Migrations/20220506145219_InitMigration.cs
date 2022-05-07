using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationalOrgs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalOrgs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemInfo_Position = table.Column<int>(type: "int", nullable: true),
                    ItemInfo_SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemInfo_TeacherInitials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemInfo_Auditorium = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangesLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOddWeek = table.Column<bool>(type: "bit", nullable: false),
                    EducationalOrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangesLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangesLists_EducationalOrgs_EducationalOrgId",
                        column: x => x.EducationalOrgId,
                        principalTable: "EducationalOrgs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationalOrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_EducationalOrgs_EducationalOrgId",
                        column: x => x.EducationalOrgId,
                        principalTable: "EducationalOrgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangesListItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangesListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangesListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangesListItems_ChangesLists_ChangesListId",
                        column: x => x.ChangesListId,
                        principalTable: "ChangesLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangesListItems_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangesListItems_ListItems_Id",
                        column: x => x.Id,
                        principalTable: "ListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleLists_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleListItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsOddWeek = table.Column<bool>(type: "bit", nullable: true),
                    ScheduleListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleListItems_ListItems_Id",
                        column: x => x.Id,
                        principalTable: "ListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleListItems_ScheduleLists_ScheduleListId",
                        column: x => x.ScheduleListId,
                        principalTable: "ScheduleLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangesListItems_ChangesListId",
                table: "ChangesListItems",
                column: "ChangesListId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangesListItems_GroupId",
                table: "ChangesListItems",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangesLists_EducationalOrgId",
                table: "ChangesLists",
                column: "EducationalOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_EducationalOrgId",
                table: "Groups",
                column: "EducationalOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleListItems_ScheduleListId",
                table: "ScheduleListItems",
                column: "ScheduleListId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleLists_GroupId",
                table: "ScheduleLists",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangesListItems");

            migrationBuilder.DropTable(
                name: "ScheduleListItems");

            migrationBuilder.DropTable(
                name: "ChangesLists");

            migrationBuilder.DropTable(
                name: "ListItems");

            migrationBuilder.DropTable(
                name: "ScheduleLists");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "EducationalOrgs");
        }
    }
}
