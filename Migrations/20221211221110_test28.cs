using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class test28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartsId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CartsId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "Carts",
                newName: "CartaId");

            migrationBuilder.RenameColumn(
                name: "CartsId",
                table: "CartItems",
                newName: "CartsCartaId");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "CartItems",
                newName: "CartIds");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartsId",
                table: "CartItems",
                newName: "IX_CartItems_CartsCartaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartsCartaId",
                table: "CartItems",
                column: "CartsCartaId",
                principalTable: "Carts",
                principalColumn: "CartaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartsCartaId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "CartaId",
                table: "Carts",
                newName: "CartId");

            migrationBuilder.RenameColumn(
                name: "CartsCartaId",
                table: "CartItems",
                newName: "CartsId");

            migrationBuilder.RenameColumn(
                name: "CartIds",
                table: "CartItems",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartsCartaId",
                table: "CartItems",
                newName: "IX_CartItems_CartsId");

            migrationBuilder.AddColumn<string>(
                name: "CartsId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartsId",
                table: "CartItems",
                column: "CartsId",
                principalTable: "Carts",
                principalColumn: "CartId");
        }
    }
}
