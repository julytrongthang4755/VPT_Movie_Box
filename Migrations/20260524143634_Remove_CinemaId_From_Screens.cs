using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPT_Movie_Box.Migrations
{
    /// <inheritdoc />
    public partial class Remove_CinemaId_From_Screens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CinemaId",
                table: "Screens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CinemaId",
                table: "Screens",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
