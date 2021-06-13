using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Identity
{
    public partial class AddUserNameAndEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                schema: "identity",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "identity",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "identity",
                table: "Users");
        }
    }
}
