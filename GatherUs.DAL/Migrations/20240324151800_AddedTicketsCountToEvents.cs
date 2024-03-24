using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedTicketsCountToEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketsLeft",
                table: "CustomEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTicketCount",
                table: "CustomEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketsLeft",
                table: "CustomEvents");

            migrationBuilder.DropColumn(
                name: "TotalTicketCount",
                table: "CustomEvents");
        }
    }
}
