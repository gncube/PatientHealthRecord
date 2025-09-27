using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Core.ClinicalDataAggregate;

public class Condition : EntityBase
{
  public PatientId PatientId { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public DateTime? OnsetDate { get; private set; }
  public DateTime? ResolvedDate { get; private set; }
  public ConditionStatus Status { get; private set; }
  public ConditionSeverity Severity { get; private set; }
  public string? Treatment { get; private set; }
  public string RecordedBy { get; private set; } = string.Empty;
  public DateTime RecordedAt { get; private set; }
  public bool IsVisibleToFamily { get; private set; } = true;

  private Condition()
  {
    PatientId = new PatientId(Guid.Empty);
  }

  public Condition(PatientId patientId, string name, string? description = null,
    DateTime? onsetDate = null, ConditionSeverity severity = ConditionSeverity.Mild,
    string? treatment = null, string recordedBy = "Self", bool isVisibleToFamily = true)
  {
    PatientId = Guard.Against.Null(patientId, nameof(patientId));
    Name = Guard.Against.NullOrEmpty(name, nameof(name));
    Description = description;
    OnsetDate = onsetDate;
    Severity = severity;
    Status = ConditionStatus.Active;
    Treatment = treatment;
    RecordedBy = recordedBy;
    RecordedAt = DateTime.UtcNow;
    IsVisibleToFamily = isVisibleToFamily;
  }

  public void Resolve(DateTime? resolvedDate = null)
  {
    Status = ConditionStatus.Resolved;
    ResolvedDate = resolvedDate ?? DateTime.UtcNow;
  }

  public void UpdateTreatment(string treatment)
  {
    Treatment = treatment;
  }

  public void UpdateSeverity(ConditionSeverity severity)
  {
    Severity = severity;
  }
}

public enum ConditionStatus
{
  Active = 1,
  Resolved = 2,
  Inactive = 3
}

public enum ConditionSeverity
{
  Mild = 1,
  Moderate = 2,
  Severe = 3
}
