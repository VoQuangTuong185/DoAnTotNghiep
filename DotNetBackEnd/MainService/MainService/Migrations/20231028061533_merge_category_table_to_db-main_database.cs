using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class merge_category_table_to_dbmain_database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalID",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "ExternalID",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
