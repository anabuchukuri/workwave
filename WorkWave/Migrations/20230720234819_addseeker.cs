using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkWave.Migrations
{
    /// <inheritdoc />
    public partial class addseeker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CoverLetter",
                table: "JobApplication",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "JobSpecificCV",
                table: "JobApplication",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "References",
                table: "JobApplication",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobSpecificCV",
                table: "JobApplication");

            migrationBuilder.DropColumn(
                name: "References",
                table: "JobApplication");

            migrationBuilder.AlterColumn<string>(
                name: "CoverLetter",
                table: "JobApplication",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
