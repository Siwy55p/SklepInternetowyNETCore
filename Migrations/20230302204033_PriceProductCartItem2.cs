using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partneraluro.Migrations
{
    /// <inheritdoc />
    public partial class PriceProductCartItem2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CenaProduktu",
                table: "CartItems",
                newName: "CenaProduktuNetto");

            migrationBuilder.AddColumn<decimal>(
                name: "CenaProduktuBrutto",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CenaProduktuBrutto",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "CenaProduktuNetto",
                table: "CartItems",
                newName: "CenaProduktu");
        }
    }
}
