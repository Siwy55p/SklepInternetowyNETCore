using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class test7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Razem",
                table: "Carts",
                newName: "RazemNetto");

            migrationBuilder.AddColumn<decimal>(
                name: "RazemBrutto",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RazemBrutto",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "RazemNetto",
                table: "Carts",
                newName: "Razem");
        }
    }
}
