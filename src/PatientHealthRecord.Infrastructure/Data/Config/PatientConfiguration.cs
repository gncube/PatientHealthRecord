using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace PatientHealthRecord.Infrastructure.Data.Config;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
  public void Configure(EntityTypeBuilder<Patient> builder)
  {
    // Configure PatientId as owned type (Value Object)
    builder.OwnsOne(p => p.PatientId, patientId =>
    {
      patientId.Property(pi => pi.Value)
        .HasColumnName("PatientId")
        .IsRequired();
    });

    // Configure basic properties with constraints
    builder.Property(p => p.Email)
        .HasMaxLength(DataSchemaConstants.EMAIL_LENGTH)
        .IsRequired();

    builder.Property(p => p.FirstName)
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();

    builder.Property(p => p.LastName)
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();

    builder.Property(p => p.DateOfBirth)
        .IsRequired();

    // Configure Gender enum
    builder.Property(p => p.Gender)
        .HasConversion<int>()
        .IsRequired();

    builder.Property(p => p.PhoneNumber)
        .HasMaxLength(DataSchemaConstants.PHONE_NUMBER_LENGTH);

    builder.Property(p => p.Relationship)
        .HasMaxLength(DataSchemaConstants.RELATIONSHIP_LENGTH)
        .HasDefaultValue("Self");

    builder.Property(p => p.IsActive)
        .IsRequired()
        .HasDefaultValue(true);

    builder.Property(p => p.CreatedAt)
        .IsRequired();

    builder.Property(p => p.LastAccessedAt);

    // Configure Emergency Contact Information
    builder.Property(p => p.EmergencyContactName)
        .HasMaxLength(DataSchemaConstants.EMERGENCY_CONTACT_NAME_LENGTH);

    builder.Property(p => p.EmergencyContactPhone)
        .HasMaxLength(DataSchemaConstants.EMERGENCY_CONTACT_PHONE_LENGTH);

    builder.Property(p => p.EmergencyContactRelationship)
        .HasMaxLength(DataSchemaConstants.EMERGENCY_CONTACT_RELATIONSHIP_LENGTH);

    // Configure Medical Information
    builder.Property(p => p.BloodType)
        .HasMaxLength(DataSchemaConstants.BLOOD_TYPE_LENGTH);

    builder.Property(p => p.Notes)
        .HasMaxLength(DataSchemaConstants.MEDICAL_NOTES_LENGTH);

    // Configure Privacy Settings
    builder.Property(p => p.ShareWithFamily)
        .IsRequired()
        .HasDefaultValue(true);

    // Store allergies as JSON for SQLite compatibility
    builder.Property(p => p.Allergies)
      .HasConversion(
        allergies => JsonSerializer.Serialize(allergies, JsonSerializerOptions.Default),
        json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default) ?? new List<string>())
      .HasColumnType("TEXT");

    builder.Property(p => p.RestrictedDataTypes)
      .HasConversion(
        types => JsonSerializer.Serialize(types, JsonSerializerOptions.Default),
        json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default) ?? new List<string>())
      .HasColumnType("TEXT");

    // Configure indexes for common queries
    builder.HasIndex(p => p.Email)
        .IsUnique()
        .HasDatabaseName("IX_Patients_Email");

    builder.HasIndex(p => new { p.FirstName, p.LastName })
        .HasDatabaseName("IX_Patients_Name");

    builder.HasIndex(p => p.IsActive)
        .HasDatabaseName("IX_Patients_IsActive");

    builder.HasIndex(p => p.PrimaryContactId)
        .HasDatabaseName("IX_Patients_PrimaryContactId");

    builder.HasIndex(p => p.DateOfBirth)
        .HasDatabaseName("IX_Patients_DateOfBirth");

    builder.HasIndex(p => p.CreatedAt)
        .HasDatabaseName("IX_Patients_CreatedAt");

    // Configure table name
    builder.ToTable("Patients");
  }
}
