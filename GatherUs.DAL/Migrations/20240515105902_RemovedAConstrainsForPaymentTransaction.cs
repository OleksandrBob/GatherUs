using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAConstrainsForPaymentTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GatherUsPaymentTransactions_CustomEventId",
                table: "GatherUsPaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_GatherUsPaymentTransactions_GuestId",
                table: "GatherUsPaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_GatherUsPaymentTransactions_OrganizerId",
                table: "GatherUsPaymentTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_GatherUsPaymentTransactions_CustomEventId",
                table: "GatherUsPaymentTransactions",
                column: "CustomEventId");

            migrationBuilder.CreateIndex(
                name: "IX_GatherUsPaymentTransactions_GuestId",
                table: "GatherUsPaymentTransactions",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_GatherUsPaymentTransactions_OrganizerId",
                table: "GatherUsPaymentTransactions",
                column: "OrganizerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GatherUsPaymentTransactions_CustomEventId",
                table: "GatherUsPaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_GatherUsPaymentTransactions_GuestId",
                table: "GatherUsPaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_GatherUsPaymentTransactions_OrganizerId",
                table: "GatherUsPaymentTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_GatherUsPaymentTransactions_CustomEventId",
                table: "GatherUsPaymentTransactions",
                column: "CustomEventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GatherUsPaymentTransactions_GuestId",
                table: "GatherUsPaymentTransactions",
                column: "GuestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GatherUsPaymentTransactions_OrganizerId",
                table: "GatherUsPaymentTransactions",
                column: "OrganizerId",
                unique: true);
        }
    }
}
