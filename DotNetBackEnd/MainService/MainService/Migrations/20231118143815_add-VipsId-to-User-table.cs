using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnTotNghiep.Migrations
{
    public partial class addVipsIdtoUsertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_VIPs_vipsId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "vipsId",
                table: "Users",
                newName: "VipsId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_vipsId",
                table: "Users",
                newName: "IX_Users_VipsId");

            migrationBuilder.AlterColumn<int>(
                name: "VipsId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_VIPs_VipsId",
                table: "Users",
                column: "VipsId",
                principalTable: "VIPs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_VIPs_VipsId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "VipsId",
                table: "Users",
                newName: "vipsId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_VipsId",
                table: "Users",
                newName: "IX_Users_vipsId");

            migrationBuilder.AlterColumn<int>(
                name: "vipsId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_VIPs_vipsId",
                table: "Users",
                column: "vipsId",
                principalTable: "VIPs",
                principalColumn: "Id");
        }
    }
}
