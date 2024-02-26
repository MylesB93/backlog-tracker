using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BacklogTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGameIDsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameIDs",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameIDs",
                table: "AspNetUsers");
        }
    }
}
