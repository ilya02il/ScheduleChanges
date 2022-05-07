using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddYearOfStudyFieldToChatsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserInfo_EducationalInfo_YearOfStudy",
                table: "Chats",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserInfo_EducationalInfo_YearOfStudy",
                table: "Chats");
        }
    }
}
