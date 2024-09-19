using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugNetCore.Dal.EFStructures.Migrations
{
    /// <inheritdoc />
    public partial class AddSeriLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Logging");

            migrationBuilder.CreateTable(
                name: "SeriLogs",
                schema: "Logging",
                columns: table => new
                {
                    Message = table.Column<string>(type: "text", nullable: true),
                    MessageTemplate = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<string>(type: "text", nullable: true),
                    RaiseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Exception = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string>(type: "jsonb", nullable: true),
                    PropsTest = table.Column<string>(type: "jsonb", nullable: true),
                    MachineName = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    ApplicationName = table.Column<string>(type: "text", nullable: true),
                    MemberName = table.Column<string>(type: "text", nullable: true),
                    LineNumber = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeriLogs",
                schema: "Logging");
        }
    }
}
