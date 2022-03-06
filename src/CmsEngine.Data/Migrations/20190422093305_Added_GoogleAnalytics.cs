using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsEngine.Data.Migrations
{
    public partial class Added_GoogleAnalytics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleAnalytics",
                table: "Websites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleAnalytics",
                table: "Websites");
        }
    }
}
