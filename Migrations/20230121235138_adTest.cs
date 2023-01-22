using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partneraluro.Migrations
{
    /// <inheritdoc />
    public partial class adTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SliderHome3",
                table: "Setting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SliderHome2",
                table: "Setting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SliderHome1",
                table: "Setting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Adress1rozliczeniowyId",
                table: "AspNetUsers",
                column: "Adress1rozliczeniowyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Adress2dostawyId",
                table: "AspNetUsers",
                column: "Adress2dostawyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Adress1rozliczeniowy_Adress1rozliczeniowyId",
                table: "AspNetUsers",
                column: "Adress1rozliczeniowyId",
                principalTable: "Adress1rozliczeniowy",
                principalColumn: "Adres1rozliczeniowyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Adress2dostawy_Adress2dostawyId",
                table: "AspNetUsers",
                column: "Adress2dostawyId",
                principalTable: "Adress2dostawy",
                principalColumn: "Adres2dostawyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Adress1rozliczeniowy_Adress1rozliczeniowyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Adress2dostawy_Adress2dostawyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Adress1rozliczeniowyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Adress2dostawyId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "SliderHome3",
                table: "Setting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SliderHome2",
                table: "Setting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SliderHome1",
                table: "Setting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
