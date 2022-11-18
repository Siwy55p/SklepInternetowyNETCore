using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class ImageForeging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Sliders_SliderImageSliderID",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_SliderImageSliderID",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "SliderImageSliderID",
                table: "Images",
                newName: "SliderIds");

            migrationBuilder.AddColumn<int>(
                name: "IdObrazek",
                table: "Sliders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageSliderID",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageSliderID",
                table: "Images",
                column: "ImageSliderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Sliders_ImageSliderID",
                table: "Images",
                column: "ImageSliderID",
                principalTable: "Sliders",
                principalColumn: "ImageSliderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Sliders_ImageSliderID",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ImageSliderID",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IdObrazek",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "ImageSliderID",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "SliderIds",
                table: "Images",
                newName: "SliderImageSliderID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_SliderImageSliderID",
                table: "Images",
                column: "SliderImageSliderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Sliders_SliderImageSliderID",
                table: "Images",
                column: "SliderImageSliderID",
                principalTable: "Sliders",
                principalColumn: "ImageSliderID");
        }
    }
}
