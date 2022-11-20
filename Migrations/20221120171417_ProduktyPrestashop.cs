using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class ProduktyPrestashop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductsPrestashop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_product = table.Column<int>(type: "int", nullable: true),
                    id_supplier = table.Column<int>(type: "int", nullable: true),
                    id_manufacturer = table.Column<int>(type: "int", nullable: true),
                    id_category_default = table.Column<int>(type: "int", nullable: true),
                    id_shop_default = table.Column<int>(type: "int", nullable: true),
                    id_tax_rules_group = table.Column<int>(type: "int", nullable: true),
                    on_sale = table.Column<byte>(type: "tinyint", nullable: true),
                    online_only = table.Column<byte>(type: "tinyint", nullable: true),
                    ean13 = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    upc = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    ecotax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    minimal_quantity = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    wholesale_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    unity = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    unit_price_ratio = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    additional_shipping_cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    reference = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    supplier_reference = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    location = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    dept = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    out_of_stock = table.Column<int>(type: "int", nullable: true),
                    quantity_discount = table.Column<byte>(type: "tinyint", nullable: true),
                    customizable = table.Column<byte>(type: "tinyint", nullable: true),
                    uploadable_files = table.Column<byte>(type: "tinyint", nullable: true),
                    text_fields = table.Column<byte>(type: "tinyint", nullable: true),
                    active = table.Column<byte>(type: "tinyint", nullable: true),
                    id_product_redirected = table.Column<int>(type: "int", nullable: true),
                    available_for_order = table.Column<byte>(type: "tinyint", nullable: true),
                    available_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    condition = table.Column<int>(type: "int", nullable: true),
                    show_price = table.Column<byte>(type: "tinyint", nullable: true),
                    indexed = table.Column<byte>(type: "tinyint", nullable: true),
                    visibility = table.Column<int>(type: "int", nullable: true),
                    cache_is_pack = table.Column<byte>(type: "tinyint", nullable: true),
                    cache_has_attachments = table.Column<byte>(type: "tinyint", nullable: true),
                    is_virtual = table.Column<byte>(type: "tinyint", nullable: true),
                    cache_default_attribute = table.Column<int>(type: "int", nullable: true),
                    date_add = table.Column<DateTime>(type: "datetime2", nullable: true),
                    date_upd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    advanced_stock_management = table.Column<byte>(type: "tinyint", nullable: true),
                    pack_stock_type = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsPrestashop", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsPrestashop");
        }
    }
}
