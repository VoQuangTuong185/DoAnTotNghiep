using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class changesomemissing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMs_Categories_CategoryId",
                table: "ProductMs");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategorieMs_CategoryMId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryMId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryMId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMs_CategorieMs_CategoryId",
                table: "ProductMs",
                column: "CategoryId",
                principalTable: "CategorieMs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMs_CategorieMs_CategoryId",
                table: "ProductMs");

            migrationBuilder.AddColumn<int>(
                name: "CategoryMId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryMId",
                table: "Products",
                column: "CategoryMId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMs_Categories_CategoryId",
                table: "ProductMs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategorieMs_CategoryMId",
                table: "Products",
                column: "CategoryMId",
                principalTable: "CategorieMs",
                principalColumn: "Id");
        }
    }
}
