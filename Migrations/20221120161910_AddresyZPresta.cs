using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class AddresyZPresta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressPrestashop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_address = table.Column<int>(type: "int", nullable: false),
                    id_country = table.Column<int>(type: "int", nullable: false),
                    id_state = table.Column<int>(type: "int", nullable: false),
                    id_customer = table.Column<int>(type: "int", nullable: false),
                    id_manufacturer = table.Column<int>(type: "int", nullable: false),
                    id_supplier = table.Column<int>(type: "int", nullable: false),
                    id_warehouse = table.Column<int>(type: "int", nullable: false),
                    alias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    other = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone_mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vat_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_add = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_upd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    active = table.Column<int>(type: "int", nullable: false),
                    deleted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressPrestashop", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressPrestashop");
        }
    }
}
