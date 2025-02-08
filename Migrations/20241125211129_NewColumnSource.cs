using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Super_Simple_Homebrew_Hoster.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "HomebrewItem",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "HomebrewItem");
        }
    }
}
