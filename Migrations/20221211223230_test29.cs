using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class test29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartsCartaId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartsCartaId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CartsCartaId",
                table: "CartItems");

            migrationBuilder.AlterColumn<string>(
                name: "CartaId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CartID",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CartsCartID",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "CartID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartsCartID",
                table: "CartItems",
                column: "CartsCartID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartsCartID",
                table: "CartItems",
                column: "CartsCartID",
                principalTable: "Carts",
                principalColumn: "CartID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartsCartID",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartsCartID",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CartID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CartsCartID",
                table: "CartItems");

            migrationBuilder.AlterColumn<string>(
                name: "CartaId",
                table: "Carts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CartsCartaId",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "CartaId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartsCartaId",
                table: "CartItems",
                column: "CartsCartaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartsCartaId",
                table: "CartItems",
                column: "CartsCartaId",
                principalTable: "Carts",
                principalColumn: "CartaId");
        }
    }
}
