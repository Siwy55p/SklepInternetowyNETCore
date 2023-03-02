using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partneraluro.Migrations
{
    /// <inheritdoc />
    public partial class PriceProductCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CenaProduktu",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CenaProduktu",
                table: "CartItems");
        }
    }
}
