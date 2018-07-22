using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsEngine.Migrations
{
    public partial class AddingContactDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Websites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Websites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Websites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "Websites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "Websites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Websites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "Websites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "Websites");
        }
    }
}
