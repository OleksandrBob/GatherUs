using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedMultipleCategoriesToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "CustomEventCategories",
                table: "CustomEvents",
                type: "integer[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomEventCategories",
                table: "CustomEvents");
        }
    }
}
