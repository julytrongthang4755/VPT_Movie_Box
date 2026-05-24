using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VPT_Movie_Box.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTable_Cinemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screens_Cinemas_CinemaId",
                table: "Screens");

            migrationBuilder.DropTable(
                name: "Cinemas");

            migrationBuilder.DropIndex(
                name: "IX_Screens_CinemaId",
                table: "Screens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    CinemaId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: false),
                    CinemaName = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinemas", x => x.CinemaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screens_CinemaId",
                table: "Screens",
                column: "CinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Screens_Cinemas_CinemaId",
                table: "Screens",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "CinemaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
