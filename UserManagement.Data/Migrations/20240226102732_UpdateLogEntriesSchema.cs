using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogEntriesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "LogEntries",
                newName: "TraceId");

            migrationBuilder.AddColumn<string>(
                name: "Ex",
                table: "LogEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Msg",
                table: "LogEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Severity",
                table: "LogEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpanId",
                table: "LogEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Template",
                table: "LogEntries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ex",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "Msg",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "SpanId",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "Template",
                table: "LogEntries");

            migrationBuilder.RenameColumn(
                name: "TraceId",
                table: "LogEntries",
                newName: "Details");
        }
    }
}
