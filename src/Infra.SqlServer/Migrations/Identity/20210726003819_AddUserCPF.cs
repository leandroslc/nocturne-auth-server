using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Identity
{
    public partial class AddUserCPF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPF",
                schema: "identity",
                table: "Users",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPF",
                schema: "identity",
                table: "Users");
        }
    }
}
