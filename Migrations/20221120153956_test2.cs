using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Idcustommer",
                table: "ContactsPrestashop",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "newsletter_date_add",
                table: "ContactsPrestashop",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "optin",
                table: "ContactsPrestashop",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Idcustommer",
                table: "ContactsPrestashop");

            migrationBuilder.DropColumn(
                name: "newsletter_date_add",
                table: "ContactsPrestashop");

            migrationBuilder.DropColumn(
                name: "optin",
                table: "ContactsPrestashop");
        }
    }
}
