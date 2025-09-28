using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Infrastructure.Data.Config;

public class ConditionConfiguration : IEntityTypeConfiguration<Condition>
{
    public void Configure(EntityTypeBuilder<Condition> builder)
    {
        // Configure PatientId as owned type (Value Object)
        builder.OwnsOne(c => c.PatientId, patientId =>
        {
            patientId.Property(pi => pi.Value)
          .HasColumnName("PatientId")
          .IsRequired();
        });

        // Configure basic properties with constraints
        builder.Property(c => c.Name)
            .HasMaxLength(DataSchemaConstants.CONDITION_NAME_LENGTH)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(DataSchemaConstants.CONDITION_DESCRIPTION_LENGTH);

        builder.Property(c => c.OnsetDate);

        builder.Property(c => c.ResolvedDate);

        // Configure Status enum
        builder.Property(c => c.Status)
            .HasConversion<int>()
            .IsRequired()
            .HasDefaultValue((int)ConditionStatus.Active);

        // Configure Severity enum
        builder.Property(c => c.Severity)
            .HasConversion<int>()
            .IsRequired()
            .HasDefaultValue((int)ConditionSeverity.Mild);

        builder.Property(c => c.Treatment)
            .HasMaxLength(DataSchemaConstants.CONDITION_TREATMENT_LENGTH);

        builder.Property(c => c.RecordedBy)
            .HasMaxLength(DataSchemaConstants.MEDICATION_RECORDED_BY_LENGTH)
            .IsRequired()
            .HasDefaultValue("Self");

        builder.Property(c => c.RecordedAt)
            .IsRequired();

        builder.Property(c => c.IsVisibleToFamily)
            .IsRequired()
            .HasDefaultValue(true);

        // Configure indexes for common queries
        builder.HasIndex(c => c.PatientId)
            .HasDatabaseName("IX_Conditions_PatientId");

        builder.HasIndex(c => new { c.PatientId, c.Status })
            .HasDatabaseName("IX_Conditions_Patient_Status");

        builder.HasIndex(c => new { c.PatientId, c.Severity })
            .HasDatabaseName("IX_Conditions_Patient_Severity");

        builder.HasIndex(c => c.RecordedAt)
            .HasDatabaseName("IX_Conditions_RecordedAt");

        // Configure table name
        builder.ToTable("Conditions");
    }
}
