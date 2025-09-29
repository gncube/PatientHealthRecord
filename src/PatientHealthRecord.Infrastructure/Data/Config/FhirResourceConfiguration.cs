using PatientHealthRecord.Core.InteroperabilityAggregate;
using PatientHealthRecord.Core.PatientAggregate;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PatientHealthRecord.Infrastructure.Data.Config;

public class FhirResourceConfiguration : IEntityTypeConfiguration<FhirResource>
{
  public void Configure(EntityTypeBuilder<FhirResource> builder)
  {
    builder.HasKey(f => f.Id);

    builder.Property(f => f.ResourceType)
        .HasMaxLength(50)
        .IsRequired();

    builder.Property(f => f.ResourceId)
        .HasMaxLength(64)
        .IsRequired();

    builder.Property(f => f.VersionId)
        .HasMaxLength(10)
        .IsRequired()
        .HasDefaultValue("1");

    builder.Property(f => f.PatientId)
        .HasConversion(
            patientId => patientId.Value,
            value => new PatientId(value))
        .IsRequired();

    builder.Property(f => f.Content)
        .HasColumnType("TEXT") // For SQLite compatibility
        .IsRequired();

    builder.Property(f => f.Status)
        .HasConversion<int>()
        .IsRequired();

    builder.Property(f => f.LastUpdated)
        .IsRequired();

    builder.Property(f => f.Source)
        .HasMaxLength(100);

    // Store metadata as JSON
    builder.Property(f => f.Metadata)
        .HasConversion(
            metadata => JsonSerializer.Serialize(metadata, JsonSerializerOptions.Default),
            json => JsonSerializer.Deserialize<Dictionary<string, string>>(json, JsonSerializerOptions.Default) ?? new Dictionary<string, string>())
        .HasColumnType("TEXT")
        .Metadata.SetValueComparer(
            new ValueComparer<Dictionary<string, string>>(
                (d1, d2) => d1 != null && d2 != null && d1.SequenceEqual(d2),
                d => d != null ? d.GetHashCode() : 0,
                d => d != null ? new Dictionary<string, string>(d) : new Dictionary<string, string>()));

    // Indexes for performance
    builder.HasIndex(f => f.PatientId);
    builder.HasIndex(f => f.ResourceType);
    builder.HasIndex(f => new { f.ResourceType, f.ResourceId });
    builder.HasIndex(f => f.LastUpdated);

    builder.ToTable("FhirResources");
  }
}
