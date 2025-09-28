namespace PatientHealthRecord.UseCases.ClinicalObservations;

public record ClinicalObservationDto(
  Guid Id,
  Guid PatientId,
  string ObservationType,
  string Value,
  string? Unit,
  DateTime RecordedAt,
  string? Notes,
  string RecordedBy,
  string Category,
  bool IsVisibleToFamily
);
