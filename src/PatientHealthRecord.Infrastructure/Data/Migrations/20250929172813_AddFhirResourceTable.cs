using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientHealthRecord.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFhirResourceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FhirResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResourceType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ResourceId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    VersionId = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false, defaultValue: "1"),
                    PatientId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Metadata = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FhirResources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FhirResources_PatientId",
                table: "FhirResources",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_FhirResources_ResourceType",
                table: "FhirResources",
                column: "ResourceType");

            migrationBuilder.CreateIndex(
                name: "IX_FhirResources_ResourceType_ResourceId",
                table: "FhirResources",
                columns: new[] { "ResourceType", "ResourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FhirResources_LastUpdated",
                table: "FhirResources",
                column: "LastUpdated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FhirResources");
        }
    }
}
