using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformSpecificChatId = table.Column<long>(type: "bigint", nullable: false),
                    PlatformHash = table.Column<long>(type: "bigint", nullable: false),
                    UserInfo_Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserInfo_EducationalInfo_EducOrgName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserInfo_EducationalInfo_GroupNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");
        }
    }
}
