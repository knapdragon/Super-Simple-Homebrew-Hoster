using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Super_Simple_Homebrew_Hoster.Migrations
{
    /// <inheritdoc />
    public partial class LinkAndContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "HomebrewItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "HomebrewItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "HomebrewItem");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "HomebrewItem");
        }
    }
}
