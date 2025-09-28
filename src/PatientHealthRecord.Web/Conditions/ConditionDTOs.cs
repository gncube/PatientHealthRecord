using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.Web.Conditions;

public record ConditionRecord(
    int Id,
    Guid PatientId,
    string Name,
    string? Description,
    DateTime? OnsetDate,
    string Severity,
    string Status,
    string? Treatment,
    DateTime? ResolvedDate,
    string RecordedBy,
    DateTime RecordedAt,
    bool IsVisibleToFamily);

public record ConditionListResponse(
    IEnumerable<ConditionRecord> Conditions);

public record CreateConditionRequest(
    Guid PatientId,
    string Name,
    string Description,
    DateTime OnsetDate,
    string Severity,
    string? Treatment,
    string RecordedBy);

public record UpdateConditionRequest(
    string? Name = null,
    string? Description = null,
    DateTime? OnsetDate = null,
    string? Severity = null,
    string? Treatment = null,
    string? RecordedBy = null);

public record ResolveConditionRequest(string ResolutionNotes);
