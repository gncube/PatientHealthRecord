using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientHealthRecord.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClinicalDataTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Patients",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "PatientId");

            migrationBuilder.CreateTable(
                name: "ClinicalObservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ObservationType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Unit = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    RecordedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    RecordedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    IsVisibleToFamily = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalObservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicalObservations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    OnsetDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ResolvedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Severity = table.Column<int>(type: "INTEGER", nullable: false),
                    Treatment = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    RecordedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "Self"),
                    RecordedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsVisibleToFamily = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conditions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Dosage = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Frequency = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Instructions = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PrescribedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Purpose = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    SideEffects = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    RecordedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "Self"),
                    IsVisibleToFamily = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medications_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalObservations_Patient_Category",
                table: "ClinicalObservations",
                columns: new[] { "PatientId", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalObservations_Patient_RecordedAt",
                table: "ClinicalObservations",
                columns: new[] { "PatientId", "RecordedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalObservations_PatientId",
                table: "ClinicalObservations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalObservations_RecordedAt",
                table: "ClinicalObservations",
                column: "RecordedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_Patient_Severity",
                table: "Conditions",
                columns: new[] { "PatientId", "Severity" });

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_Patient_Status",
                table: "Conditions",
                columns: new[] { "PatientId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_PatientId",
                table: "Conditions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_RecordedAt",
                table: "Conditions",
                column: "RecordedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_Patient_StartDate",
                table: "Medications",
                columns: new[] { "PatientId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Medications_Patient_Status",
                table: "Medications",
                columns: new[] { "PatientId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Medications_PatientId",
                table: "Medications",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_StartDate",
                table: "Medications",
                column: "StartDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicalObservations");

            migrationBuilder.DropTable(
                name: "Conditions");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Patients",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "Id");
        }
    }
}
