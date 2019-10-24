using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsEngine.Data.Migrations
{
    public partial class AddingEmailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    VanityId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    UserCreated = table.Column<string>(maxLength: 20, nullable: true),
                    UserModified = table.Column<string>(maxLength: 20, nullable: true),
                    From = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(maxLength: 150, nullable: false),
                    Message = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emails");
        }
    }
}
