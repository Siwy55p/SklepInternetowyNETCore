using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partneraluro.Migrations
{
    /// <inheritdoc />
    public partial class ps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Adress1rozliczeniowy_Adress1rozliczeniowyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Adress2dostawy_Adress2dostawyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfileDzialalnosci_IdProfilDzialalnosci",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Adress1rozliczeniowyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Adress2dostawyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdProfilDzialalnosci",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "IdProfilDzialalnosci",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdProfilDzialalnosci",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Adress1rozliczeniowyId",
                table: "AspNetUsers",
                column: "Adress1rozliczeniowyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Adress2dostawyId",
                table: "AspNetUsers",
                column: "Adress2dostawyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdProfilDzialalnosci",
                table: "AspNetUsers",
                column: "IdProfilDzialalnosci");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfileDzialalnosci_IdProfilDzialalnosci",
                table: "AspNetUsers",
                column: "IdProfilDzialalnosci",
                principalTable: "ProfileDzialalnosci",
                principalColumn: "Id");
        }
    }
}
