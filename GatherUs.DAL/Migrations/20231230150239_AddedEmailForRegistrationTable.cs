using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmailForRegistrationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMailConfirmed",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "EmailForRegistrations",
                columns: table => new
                {
                    Email = table.Column<string>(type: "text", nullable: false),
                    ConfirmationCode = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailForRegistrations", x => x.Email);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailForRegistrations");

            migrationBuilder.AddColumn<bool>(
                name: "IsMailConfirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
