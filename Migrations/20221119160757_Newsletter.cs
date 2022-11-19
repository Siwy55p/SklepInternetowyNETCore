using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class Newsletter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sliders_Images_ObrazekSlideraImageId",
                table: "Sliders");

            migrationBuilder.DropIndex(
                name: "IX_Sliders_ObrazekSlideraImageId",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "ObrazekSlideraImageId",
                table: "Sliders");

            migrationBuilder.AddColumn<int>(
                name: "NewsletterID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Newsletter",
                columns: table => new
                {
                    NewsletterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessagerBody = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newsletter", x => x.NewsletterID);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Newsletter_NewsletterID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Newsletter");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NewsletterID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NewsletterID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ObrazekSlideraImageId",
                table: "Sliders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sliders_ObrazekSlideraImageId",
                table: "Sliders",
                column: "ObrazekSlideraImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sliders_Images_ObrazekSlideraImageId",
                table: "Sliders",
                column: "ObrazekSlideraImageId",
                principalTable: "Images",
                principalColumn: "ImageId");
        }
    }
}
