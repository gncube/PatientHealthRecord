namespace PatientHealthRecord.UseCases.Medications;

public record MedicationDto(
  int Id,
  Guid PatientId,
  string Name,
  string? Dosage,
  string? Frequency,
  string? Instructions,
  DateTime StartDate,
  DateTime? EndDate,
  string Status,
  string? PrescribedBy,
  string? Purpose,
  string? SideEffects,
  string RecordedBy,
  bool IsVisibleToFamily
);
