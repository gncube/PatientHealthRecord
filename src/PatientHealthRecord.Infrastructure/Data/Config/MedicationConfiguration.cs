using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Infrastructure.Data.Config;

public class MedicationConfiguration : IEntityTypeConfiguration<Medication>
{
    public void Configure(EntityTypeBuilder<Medication> builder)
    {
        // Configure PatientId as owned type (Value Object)
        builder.OwnsOne(m => m.PatientId, patientId =>
        {
            patientId.Property(pi => pi.Value)
          .HasColumnName("PatientId")
          .IsRequired();
        });

        // Configure basic properties with constraints
        builder.Property(m => m.Name)
            .HasMaxLength(DataSchemaConstants.MEDICATION_NAME_LENGTH)
            .IsRequired();

        builder.Property(m => m.Dosage)
            .HasMaxLength(DataSchemaConstants.MEDICATION_DOSAGE_LENGTH);

        builder.Property(m => m.Frequency)
            .HasMaxLength(DataSchemaConstants.MEDICATION_FREQUENCY_LENGTH);

        builder.Property(m => m.Instructions)
            .HasMaxLength(DataSchemaConstants.MEDICATION_INSTRUCTIONS_LENGTH);

        builder.Property(m => m.StartDate)
            .IsRequired();

        builder.Property(m => m.EndDate);

        // Configure Status enum
        builder.Property(m => m.Status)
            .HasConversion<int>()
            .IsRequired()
            .HasDefaultValue((int)MedicationStatus.Active);

        builder.Property(m => m.PrescribedBy)
            .HasMaxLength(DataSchemaConstants.MEDICATION_PRESCRIBED_BY_LENGTH);

        builder.Property(m => m.Purpose)
            .HasMaxLength(DataSchemaConstants.MEDICATION_PURPOSE_LENGTH);

        builder.Property(m => m.SideEffects)
            .HasMaxLength(DataSchemaConstants.MEDICATION_SIDE_EFFECTS_LENGTH);

        builder.Property(m => m.RecordedBy)
            .HasMaxLength(DataSchemaConstants.MEDICATION_RECORDED_BY_LENGTH)
            .IsRequired()
            .HasDefaultValue("Self");

        builder.Property(m => m.IsVisibleToFamily)
            .IsRequired()
            .HasDefaultValue(true);

        // Configure indexes for common queries
        builder.HasIndex(m => m.PatientId)
            .HasDatabaseName("IX_Medications_PatientId");

        builder.HasIndex(m => new { m.PatientId, m.Status })
            .HasDatabaseName("IX_Medications_Patient_Status");

        builder.HasIndex(m => new { m.PatientId, m.StartDate })
            .HasDatabaseName("IX_Medications_Patient_StartDate");

        builder.HasIndex(m => m.StartDate)
            .HasDatabaseName("IX_Medications_StartDate");

        // Configure table name
        builder.ToTable("Medications");
    }
}
