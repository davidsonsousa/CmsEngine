using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CmsEngine.Data.Migrations
{
    public partial class ShadowPropertiesForAuditing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateReceived",
                table: "Emails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "Emails");
        }
    }
}
