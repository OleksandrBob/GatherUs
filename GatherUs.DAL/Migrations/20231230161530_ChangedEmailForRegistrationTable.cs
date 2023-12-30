using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GatherUs.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEmailForRegistrationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailForRegistrations",
                table: "EmailForRegistrations");

            migrationBuilder.AlterColumn<int>(
                name: "ConfirmationCode",
                table: "EmailForRegistrations",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EmailForRegistrations",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "EmailForRegistrations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_EmailForRegistrations_Email",
                table: "EmailForRegistrations",
                column: "Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailForRegistrations",
                table: "EmailForRegistrations",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_EmailForRegistrations_Email",
                table: "EmailForRegistrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailForRegistrations",
                table: "EmailForRegistrations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EmailForRegistrations");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "EmailForRegistrations");

            migrationBuilder.AlterColumn<short>(
                name: "ConfirmationCode",
                table: "EmailForRegistrations",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailForRegistrations",
                table: "EmailForRegistrations",
                column: "Email");
        }
    }
}
