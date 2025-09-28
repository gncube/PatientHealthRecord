using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.Web.Medications;

public record MedicationRecord(
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
    bool IsVisibleToFamily);

public record MedicationListResponse(
    IEnumerable<MedicationRecord> Medications);

public record CreateMedicationRequest(
    Guid PatientId,
    string Name,
    string? Dosage,
    string? Frequency,
    string? Instructions,
    DateTime? StartDate,
    string? PrescribedBy,
    string? Purpose,
    string RecordedBy,
    bool IsVisibleToFamily);

public record UpdateMedicationRequest(
    string Name,
    string? Dosage,
    string? Frequency,
    string? Instructions,
    DateTime StartDate,
    string? PrescribedBy,
    string? Purpose,
    string RecordedBy,
    bool IsVisibleToFamily);

public record StopMedicationRequest(
    DateTime? EndDate,
    string? Reason);

public record CompleteMedicationRequest(DateTime? EndDate);

public record RecordSideEffectRequest(string SideEffect);
