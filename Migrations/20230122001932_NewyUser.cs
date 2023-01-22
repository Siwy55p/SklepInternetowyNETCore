using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partneraluro.Migrations
{
    /// <inheritdoc />
    public partial class NewyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Nowy",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nowy",
                table: "AspNetUsers");
        }
    }
}
