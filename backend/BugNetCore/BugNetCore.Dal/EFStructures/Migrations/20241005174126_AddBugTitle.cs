using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugNetCore.Dal.EFStructures.Migrations
{
    /// <inheritdoc />
    public partial class AddBugTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Bugs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Bugs");
        }
    }
}
