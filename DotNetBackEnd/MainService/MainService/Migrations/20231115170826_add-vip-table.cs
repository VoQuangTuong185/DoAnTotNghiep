using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class addviptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "vipsId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VIPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceFrom = table.Column<double>(type: "float", nullable: false),
                    PriceTo = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VIPs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_vipsId",
                table: "Users",
                column: "vipsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_VIPs_vipsId",
                table: "Users",
                column: "vipsId",
                principalTable: "VIPs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_VIPs_vipsId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "VIPs");

            migrationBuilder.DropIndex(
                name: "IX_Users_vipsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "vipsId",
                table: "Users");
        }
    }
}
