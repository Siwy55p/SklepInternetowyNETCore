using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partner_aluro.Migrations
{
    public partial class slider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SliderImageSliderID",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    ImageSliderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObrazekSlideraImageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.ImageSliderID);
                    table.ForeignKey(
                        name: "FK_Sliders_Images_ObrazekSlideraImageId",
                        column: x => x.ObrazekSlideraImageId,
                        principalTable: "Images",
                        principalColumn: "ImageId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_SliderImageSliderID",
                table: "Images",
                column: "SliderImageSliderID");

            migrationBuilder.CreateIndex(
                name: "IX_Sliders_ObrazekSlideraImageId",
                table: "Sliders",
                column: "ObrazekSlideraImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Sliders_SliderImageSliderID",
                table: "Images",
                column: "SliderImageSliderID",
                principalTable: "Sliders",
                principalColumn: "ImageSliderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Sliders_SliderImageSliderID",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropIndex(
                name: "IX_Images_SliderImageSliderID",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SliderImageSliderID",
                table: "Images");
        }
    }
}
