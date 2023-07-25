using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkWave.Migrations
{
    /// <inheritdoc />
    public partial class seedstamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "JobApplication",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0cbcd0ec-7875-40f5-84ff-83cdeaa95c39");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7b8178d5-5d75-4fe4-b757-197e8cc6dfdb", "AQAAAAEAACcQAAAAEEebugqWkTtPm+4mM3IIe/veRwOli3XWoooxnBTuu8TLDHJK0PwXMHDGk73r8+bLMw==", "028457b3-f755-45fe-b85e-070c8141e9a0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "JobApplication");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "722954d9-58d1-4a29-a671-6bea84d2d034");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dfefa1e5-c482-4f35-9fdd-ac90549830df", "AQAAAAEAACcQAAAAEMSCmZaQr3eBJED+uZzJmipzKBOlUsdlbPOIJD6koE8wCmwoY5HdyxQ5hC8s+V7eNw==", null });
        }
    }
}
