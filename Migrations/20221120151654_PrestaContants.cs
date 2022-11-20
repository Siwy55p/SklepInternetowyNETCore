using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class PrestaContants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactsPrestashop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_shop_group = table.Column<int>(type: "int", nullable: false),
                    id_shop = table.Column<int>(type: "int", nullable: false),
                    id_gender = table.Column<int>(type: "int", nullable: false),
                    id_default_group = table.Column<int>(type: "int", nullable: false),
                    id_id_lang = table.Column<int>(type: "int", nullable: false),
                    id_risk = table.Column<int>(type: "int", nullable: false),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    secure_key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    active = table.Column<int>(type: "int", nullable: false),
                    is_quest = table.Column<int>(type: "int", nullable: false),
                    deleted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactsPrestashop", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactsPrestashop");
        }
    }
}
