using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class produktWymiaryWew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GlebokoscWewnetrznaProduktu",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SzerokoscWewnetrznaProduktu",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WysokoscWewnetrznaProduktu",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlebokoscWewnetrznaProduktu",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SzerokoscWewnetrznaProduktu",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WysokoscWewnetrznaProduktu",
                table: "Products");
        }
    }
}
