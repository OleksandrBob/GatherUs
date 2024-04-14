using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaymentTransactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GatherUsPaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuestId = table.Column<int>(type: "integer", nullable: false),
                    OrganizerId = table.Column<int>(type: "integer", nullable: false),
                    CustomEventId = table.Column<int>(type: "integer", nullable: false),
                    TransactionAmount = table.Column<double>(type: "double precision", nullable: false),
                    Fee = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatherUsPaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GatherUsPaymentTransactions_CustomEvents_CustomEventId",
                        column: x => x.CustomEventId,
                        principalTable: "CustomEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GatherUsPaymentTransactions_Users_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GatherUsPaymentTransactions_Users_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GatherUsPaymentTransactions");
        }
    }
}
