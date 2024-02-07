using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomEventGuestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomEventGuest_CustomEvents_PaidEventsId",
                table: "CustomEventGuest");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomEventGuest_Users_AttendantsId",
                table: "CustomEventGuest");

            migrationBuilder.RenameColumn(
                name: "PaidEventsId",
                table: "CustomEventGuest",
                newName: "CustomEventId");

            migrationBuilder.RenameColumn(
                name: "AttendantsId",
                table: "CustomEventGuest",
                newName: "GuestId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomEventGuest_PaidEventsId",
                table: "CustomEventGuest",
                newName: "IX_CustomEventGuest_CustomEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEventGuest_CustomEvents_CustomEventId",
                table: "CustomEventGuest",
                column: "CustomEventId",
                principalTable: "CustomEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEventGuest_Users_GuestId",
                table: "CustomEventGuest",
                column: "GuestId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomEventGuest_CustomEvents_CustomEventId",
                table: "CustomEventGuest");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomEventGuest_Users_GuestId",
                table: "CustomEventGuest");

            migrationBuilder.RenameColumn(
                name: "CustomEventId",
                table: "CustomEventGuest",
                newName: "PaidEventsId");

            migrationBuilder.RenameColumn(
                name: "GuestId",
                table: "CustomEventGuest",
                newName: "AttendantsId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomEventGuest_CustomEventId",
                table: "CustomEventGuest",
                newName: "IX_CustomEventGuest_PaidEventsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEventGuest_CustomEvents_PaidEventsId",
                table: "CustomEventGuest",
                column: "PaidEventsId",
                principalTable: "CustomEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomEventGuest_Users_AttendantsId",
                table: "CustomEventGuest",
                column: "AttendantsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
