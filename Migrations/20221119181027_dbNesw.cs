using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class dbNesw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Newsletter_NewsletterID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NewsletterID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NewsletterID",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewsletterID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NewsletterID",
                table: "AspNetUsers",
                column: "NewsletterID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Newsletter_NewsletterID",
                table: "AspNetUsers",
                column: "NewsletterID",
                principalTable: "Newsletter",
                principalColumn: "NewsletterID");
        }
    }
}
