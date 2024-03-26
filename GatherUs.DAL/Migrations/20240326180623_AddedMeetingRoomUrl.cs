using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedMeetingRoomUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostRoomUrl",
                table: "CustomEvents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomUrl",
                table: "CustomEvents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostRoomUrl",
                table: "CustomEvents");

            migrationBuilder.DropColumn(
                name: "RoomUrl",
                table: "CustomEvents");
        }
    }
}
