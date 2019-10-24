using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsEngine.Data.Migrations
{
    public partial class GoogleRecaptchaFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleRecaptchaSecretKey",
                table: "Websites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleRecaptchaSiteKey",
                table: "Websites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleRecaptchaSecretKey",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "GoogleRecaptchaSiteKey",
                table: "Websites");
        }
    }
}
