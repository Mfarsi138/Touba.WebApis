using Microsoft.EntityFrameworkCore.Migrations;

namespace Touba.WebApis.API.Data.Migrations._DbContext
{
    public partial class AddMainObjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MainObjectId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true,
                defaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainObjectId",
                table: "AspNetUsers");
        }
    }
}
