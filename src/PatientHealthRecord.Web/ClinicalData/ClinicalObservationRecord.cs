namespace PatientHealthRecord.Web.ClinicalData;

public record ClinicalObservationRecord(
    int Id,
    Guid PatientId,
    string ObservationType,
    string Value,
    string? Unit,
    DateTimeOffset RecordedAt,
    string? RecordedBy,
    string Category,
    string? Notes,
    bool IsVisibleToFamily);
