using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientHealthRecord.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Relationship = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    PrimaryContactId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastAccessedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    EmergencyContactRelationship = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    BloodType = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    Allergies = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    ShareWithFamily = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    RestrictedDataTypes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CreatedAt",
                table: "Patients",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DateOfBirth",
                table: "Patients",
                column: "DateOfBirth");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Email",
                table: "Patients",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_IsActive",
                table: "Patients",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Name",
                table: "Patients",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PrimaryContactId",
                table: "Patients",
                column: "PrimaryContactId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
