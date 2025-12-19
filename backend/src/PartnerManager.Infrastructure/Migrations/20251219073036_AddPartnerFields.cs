using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PartnerManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartnerFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Only add new columns; do not alter existing identity columns

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Partners",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Partners",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Partners",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            // No changes to existing Id columns
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Partners");

            // No down changes for Id columns
        }
    }
}
