using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkWave.Migrations
{
    /// <inheritdoc />
    public partial class seedcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dfefa1e5-c482-4f35-9fdd-ac90549830df", "AQAAAAEAACcQAAAAEMSCmZaQr3eBJED+uZzJmipzKBOlUsdlbPOIJD6koE8wCmwoY5HdyxQ5hC8s+V7eNw==" });

            migrationBuilder.InsertData(
                table: "JobCategory",
                columns: new[] { "JobCategoryId", "Name" },
                values: new object[] { 1, "Sales" });

            migrationBuilder.InsertData(
                table: "JobType",
                columns: new[] { "JobTypeId", "Name" },
                values: new object[] { 1, "Full-Time" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobCategory",
                keyColumn: "JobCategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "JobType",
                keyColumn: "JobTypeId",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7749dcea-cf8a-4359-94d8-ddb99521f55c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7f04a0d9-aa17-40f1-8d96-fc98a6000ed1", "AQAAAAEAACcQAAAAEODHtoeO3U1m2MzUfxajIATUjc591MZluQbohO4nVFzmjOmISp9R9e1EzYgnRvaxUg==" });
        }
    }
}
