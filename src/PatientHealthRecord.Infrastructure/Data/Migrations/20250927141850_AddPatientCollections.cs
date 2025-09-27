using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientHealthRecord.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientCollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Relationship",
                table: "Patients",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                defaultValue: "Self",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Relationship",
                table: "Patients",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "Self");
        }
    }
}
