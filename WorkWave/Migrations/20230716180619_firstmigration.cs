using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkWave.Migrations
{
    /// <inheritdoc />
    public partial class firstmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobCategory",
                columns: table => new
                {
                    JobCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategory", x => x.JobCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "JobType",
                columns: table => new
                {
                    JobTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobType", x => x.JobTypeId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Employer",
                columns: table => new
                {
                    EmployerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employer", x => x.EmployerId);
                    table.ForeignKey(
                        name: "FK_Employer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobSeeker",
                columns: table => new
                {
                    JobSeekerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LinkedInProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GithubProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Education = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSeeker", x => x.JobSeekerId);
                    table.ForeignKey(
                        name: "FK_JobSeeker_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobOpening",
                columns: table => new
                {
                    JobOpeningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployerId = table.Column<int>(type: "int", nullable: true),
                    JobTypeId = table.Column<int>(type: "int", nullable: true),
                    JobDetailsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOpening", x => x.JobOpeningId);
                    table.ForeignKey(
                        name: "FK_JobOpening_Employer_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employer",
                        principalColumn: "EmployerId");
                    table.ForeignKey(
                        name: "FK_JobOpening_JobType_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobType",
                        principalColumn: "JobTypeId");
                });

            migrationBuilder.CreateTable(
                name: "JobApplication",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoverLetter = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    JobSeekerId = table.Column<int>(type: "int", nullable: false),
                    JobOpeningId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplication", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_JobApplication_JobOpening_JobOpeningId",
                        column: x => x.JobOpeningId,
                        principalTable: "JobOpening",
                        principalColumn: "JobOpeningId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobApplication_JobSeeker_JobSeekerId",
                        column: x => x.JobSeekerId,
                        principalTable: "JobSeeker",
                        principalColumn: "JobSeekerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobDetails",
                columns: table => new
                {
                    JobDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmploymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequiredExperience = table.Column<int>(type: "int", nullable: false),
                    Qualifications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Responsibilities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyCulture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationInstructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfOpenings = table.Column<int>(type: "int", nullable: false),
                    IsFullTime = table.Column<bool>(type: "bit", nullable: false),
                    IsRemote = table.Column<bool>(type: "bit", nullable: false),
                    JobOpeningId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDetails", x => x.JobDetailsId);
                    table.ForeignKey(
                        name: "FK_JobDetails_JobOpening_JobOpeningId",
                        column: x => x.JobOpeningId,
                        principalTable: "JobOpening",
                        principalColumn: "JobOpeningId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpeningCategory",
                columns: table => new
                {
                    JobOpeningId = table.Column<int>(type: "int", nullable: false),
                    JobCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningCategory", x => new { x.JobOpeningId, x.JobCategoryId });
                    table.ForeignKey(
                        name: "FK_OpeningCategory_JobCategory_JobCategoryId",
                        column: x => x.JobCategoryId,
                        principalTable: "JobCategory",
                        principalColumn: "JobCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpeningCategory_JobOpening_JobOpeningId",
                        column: x => x.JobOpeningId,
                        principalTable: "JobOpening",
                        principalColumn: "JobOpeningId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employer_UserId",
                table: "Employer",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplication_JobOpeningId",
                table: "JobApplication",
                column: "JobOpeningId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplication_JobSeekerId",
                table: "JobApplication",
                column: "JobSeekerId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDetails_JobOpeningId",
                table: "JobDetails",
                column: "JobOpeningId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOpening_EmployerId",
                table: "JobOpening",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpening_JobTypeId",
                table: "JobOpening",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeeker_UserId",
                table: "JobSeeker",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpeningCategory_JobCategoryId",
                table: "OpeningCategory",
                column: "JobCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobApplication");

            migrationBuilder.DropTable(
                name: "JobDetails");

            migrationBuilder.DropTable(
                name: "OpeningCategory");

            migrationBuilder.DropTable(
                name: "JobSeeker");

            migrationBuilder.DropTable(
                name: "JobCategory");

            migrationBuilder.DropTable(
                name: "JobOpening");

            migrationBuilder.DropTable(
                name: "Employer");

            migrationBuilder.DropTable(
                name: "JobType");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
