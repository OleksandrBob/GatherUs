using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreatedEventAndInvite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OrganizerId = table.Column<int>(type: "integer", nullable: false),
                    StartTimeUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MinRequiredAge = table.Column<byte>(type: "smallint", nullable: false),
                    TicketPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CustomEventType = table.Column<byte>(type: "smallint", nullable: false),
                    CustomEventLocationType = table.Column<byte>(type: "smallint", nullable: false),
                    CustomEventCategory = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomEvents_Users_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttendanceInvites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomEventId = table.Column<int>(type: "integer", nullable: false),
                    GuestId = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    InviteStatus = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceInvites_CustomEvents_CustomEventId",
                        column: x => x.CustomEventId,
                        principalTable: "CustomEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceInvites_Users_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomEventGuest",
                columns: table => new
                {
                    AttendantsId = table.Column<int>(type: "integer", nullable: false),
                    PaidEventsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomEventGuest", x => new { x.AttendantsId, x.PaidEventsId });
                    table.ForeignKey(
                        name: "FK_CustomEventGuest_CustomEvents_PaidEventsId",
                        column: x => x.PaidEventsId,
                        principalTable: "CustomEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomEventGuest_Users_AttendantsId",
                        column: x => x.AttendantsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceInvites_CustomEventId",
                table: "AttendanceInvites",
                column: "CustomEventId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceInvites_GuestId",
                table: "AttendanceInvites",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomEventGuest_PaidEventsId",
                table: "CustomEventGuest",
                column: "PaidEventsId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomEvents_OrganizerId",
                table: "CustomEvents",
                column: "OrganizerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceInvites");

            migrationBuilder.DropTable(
                name: "CustomEventGuest");

            migrationBuilder.DropTable(
                name: "CustomEvents");
        }
    }
}
