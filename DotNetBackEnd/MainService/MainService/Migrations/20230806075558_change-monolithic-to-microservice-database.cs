using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class changemonolithictomicroservicedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryMId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductMId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategorieMs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalID = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorieMs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductMs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Quanity = table.Column<int>(type: "int", nullable: false),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMs_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMs_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryMId",
                table: "Products",
                column: "CategoryMId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductMId",
                table: "Carts",
                column: "ProductMId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMs_BrandId",
                table: "ProductMs",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMs_CategoryId",
                table: "ProductMs",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_ProductMs_ProductMId",
                table: "Carts",
                column: "ProductMId",
                principalTable: "ProductMs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategorieMs_CategoryMId",
                table: "Products",
                column: "CategoryMId",
                principalTable: "CategorieMs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ProductMs_ProductMId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategorieMs_CategoryMId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "CategorieMs");

            migrationBuilder.DropTable(
                name: "ProductMs");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryMId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ProductMId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CategoryMId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductMId",
                table: "Carts");
        }
    }
}
