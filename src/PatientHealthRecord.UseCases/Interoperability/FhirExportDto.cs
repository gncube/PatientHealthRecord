namespace PatientHealthRecord.UseCases.Interoperability;

public record FhirExportDto(
    string BundleId,
    string PatientName,
    string Format,
    string MimeType,
    string Content,
    int ResourceCount,
    DateTimeOffset ExportedAt
);
