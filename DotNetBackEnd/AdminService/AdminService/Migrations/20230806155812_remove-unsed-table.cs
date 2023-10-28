using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class removeunsedtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ProductMs_ProductMId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "ProductMs");

            migrationBuilder.DropTable(
                name: "CategorieMs");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ProductMId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ProductMId",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "ExternalID",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalID = table.Column<int>(type: "int", nullable: false)
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
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quanity = table.Column<int>(type: "int", nullable: false),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_ProductMs_CategorieMs_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategorieMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
        }
    }
}
