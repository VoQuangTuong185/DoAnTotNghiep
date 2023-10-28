using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class add_ImageDetail_column_in_Product_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageDetail",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageDetail",
                table: "Products");
        }
    }
}
