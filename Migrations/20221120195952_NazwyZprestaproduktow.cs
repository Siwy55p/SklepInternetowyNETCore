using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class NazwyZprestaproduktow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductsNamePrestashop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_product = table.Column<int>(type: "int", nullable: true),
                    id_shop = table.Column<int>(type: "int", nullable: true),
                    id_lang = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description_short = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    link_rewrite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    meta_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    meta_keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    meta_title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    available_now = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    available_later = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsNamePrestashop", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsNamePrestashop");
        }
    }
}
