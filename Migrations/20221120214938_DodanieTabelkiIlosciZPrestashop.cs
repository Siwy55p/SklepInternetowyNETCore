using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class DodanieTabelkiIlosciZPrestashop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductsQuantityPrestashop",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_stock_available = table.Column<int>(type: "int", nullable: false),
                    id_product = table.Column<int>(type: "int", nullable: false),
                    id_product_attribute = table.Column<int>(type: "int", nullable: false),
                    id_shop = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    depends_on_stock = table.Column<int>(type: "int", nullable: false),
                    out_of_stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsQuantityPrestashop", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsQuantityPrestashop");
        }
    }
}
