using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Infrastructure.Data.Config;

public class ClinicalObservationConfiguration : IEntityTypeConfiguration<ClinicalObservation>
{
    public void Configure(EntityTypeBuilder<ClinicalObservation> builder)
    {
        // Configure PatientId as owned type (Value Object)
        builder.OwnsOne(o => o.PatientId, patientId =>
        {
            patientId.Property(pi => pi.Value)
          .HasColumnName("PatientId")
          .IsRequired();
        });

        // Configure basic properties with constraints
        builder.Property(o => o.ObservationType)
            .HasMaxLength(DataSchemaConstants.OBSERVATION_TYPE_LENGTH)
            .IsRequired();

        builder.Property(o => o.Value)
            .HasMaxLength(DataSchemaConstants.OBSERVATION_VALUE_LENGTH)
            .IsRequired();

        builder.Property(o => o.Unit)
            .HasMaxLength(DataSchemaConstants.OBSERVATION_UNIT_LENGTH);

        builder.Property(o => o.RecordedAt)
            .IsRequired();

        builder.Property(o => o.Notes)
            .HasMaxLength(DataSchemaConstants.OBSERVATION_NOTES_LENGTH);

        builder.Property(o => o.RecordedBy)
            .HasMaxLength(DataSchemaConstants.OBSERVATION_RECORDED_BY_LENGTH)
            .IsRequired();

        // Configure Category enum
        builder.Property(o => o.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(o => o.IsVisibleToFamily)
            .IsRequired()
            .HasDefaultValue(true);

        // Configure indexes for common queries
        builder.HasIndex(o => o.PatientId)
            .HasDatabaseName("IX_ClinicalObservations_PatientId");

        builder.HasIndex(o => new { o.PatientId, o.Category })
            .HasDatabaseName("IX_ClinicalObservations_Patient_Category");

        builder.HasIndex(o => new { o.PatientId, o.RecordedAt })
            .HasDatabaseName("IX_ClinicalObservations_Patient_RecordedAt");

        builder.HasIndex(o => o.RecordedAt)
            .HasDatabaseName("IX_ClinicalObservations_RecordedAt");

        // Configure table name
        builder.ToTable("ClinicalObservations");
    }
}
