using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamayoz.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachmentToServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentFileName",
                table: "ServiceRequests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "ServiceRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AttachmentSize",
                table: "ServiceRequests",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentFileName",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "AttachmentSize",
                table: "ServiceRequests");
        }
    }
}
