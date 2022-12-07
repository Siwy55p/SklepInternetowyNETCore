using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class cartw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "CartItems");

            migrationBuilder.AddColumn<string>(
                name: "UserIDId",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserIDId",
                table: "CartItems",
                column: "UserIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AspNetUsers_UserIDId",
                table: "CartItems",
                column: "UserIDId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AspNetUsers_UserIDId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_UserIDId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UserIDId",
                table: "CartItems");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
