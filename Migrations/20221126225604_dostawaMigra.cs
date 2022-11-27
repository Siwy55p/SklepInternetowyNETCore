using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class dostawaMigra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SposobDostawy",
                table: "Orders",
                newName: "MetodaDostawy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MetodaDostawy",
                table: "Orders",
                newName: "SposobDostawy");
        }
    }
}
