using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class dodaniePozycjiDoContactPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "birthday",
                table: "ContactsPrestashop",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_add",
                table: "ContactsPrestashop",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_upd",
                table: "ContactsPrestashop",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birthday",
                table: "ContactsPrestashop");

            migrationBuilder.DropColumn(
                name: "date_add",
                table: "ContactsPrestashop");

            migrationBuilder.DropColumn(
                name: "date_upd",
                table: "ContactsPrestashop");
        }
    }
}
