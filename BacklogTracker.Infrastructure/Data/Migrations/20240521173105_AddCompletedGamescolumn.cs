using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BacklogTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedGamescolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletedGameIDs",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedGameIDs",
                table: "AspNetUsers");
        }
    }
}
