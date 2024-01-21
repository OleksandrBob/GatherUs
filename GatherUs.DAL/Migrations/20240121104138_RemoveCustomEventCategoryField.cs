using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCustomEventCategoryField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomEventCategory",
                table: "CustomEvents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomEventCategory",
                table: "CustomEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
