using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsEngine.Migrations
{
    public partial class AddingArticleLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleLimit",
                table: "Websites",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleLimit",
                table: "Websites");
        }
    }
}
