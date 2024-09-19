using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugNetCore.Dal.EFStructures.Migrations
{
    /// <inheritdoc />
    public partial class AddConditionalBugOrSupportRequestRelationsToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BugId",
                table: "Notifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupportRequestId",
                table: "Notifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BugId",
                table: "Notifications",
                column: "BugId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SupportRequestId",
                table: "Notifications",
                column: "SupportRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Bugs_BugId",
                table: "Notifications",
                column: "BugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_SupportRequests_SupportRequestId",
                table: "Notifications",
                column: "SupportRequestId",
                principalTable: "SupportRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Bugs_BugId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_SupportRequests_SupportRequestId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_BugId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SupportRequestId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "BugId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SupportRequestId",
                table: "Notifications");
        }
    }
}
