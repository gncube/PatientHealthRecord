using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Core.Entities.ClinicalDataAggregate;

public class Medication : EntityBase
{
  public PatientId PatientId { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public string? Dosage { get; private set; }
  public string? Frequency { get; private set; }
  public string? Instructions { get; private set; }
  public DateTime StartDate { get; private set; }
  public DateTime? EndDate { get; private set; }
  public MedicationStatus Status { get; private set; }
  public string? PrescribedBy { get; private set; }
  public string? Purpose { get; private set; }
  public string? SideEffects { get; private set; }
  public string RecordedBy { get; private set; } = string.Empty;
  public bool IsVisibleToFamily { get; private set; } = true;

  private Medication()
  {
    PatientId = new PatientId(Guid.Empty);
  }

  public Medication(PatientId patientId, string name, string? dosage = null,
    string? frequency = null, DateTime? startDate = null, string? prescribedBy = null,
    string? purpose = null, string recordedBy = "Self", bool isVisibleToFamily = true)
  {
    PatientId = Guard.Against.Null(patientId, nameof(patientId));
    Name = Guard.Against.NullOrEmpty(name, nameof(name));
    Dosage = dosage;
    Frequency = frequency;
    StartDate = startDate ?? DateTime.UtcNow;
    Status = MedicationStatus.Active;
    PrescribedBy = prescribedBy;
    Purpose = purpose;
    RecordedBy = recordedBy;
    IsVisibleToFamily = isVisibleToFamily;
  }

  public void Stop(DateTime? endDate = null, string? reason = null)
  {
    Status = MedicationStatus.Stopped;
    EndDate = endDate ?? DateTime.UtcNow;
    if (!string.IsNullOrEmpty(reason))
    {
      Instructions = string.IsNullOrEmpty(Instructions) ? reason : $"{Instructions}\nStopped: {reason}";
    }
  }

  public void Complete(DateTime? endDate = null)
  {
    Status = MedicationStatus.Completed;
    EndDate = endDate ?? DateTime.UtcNow;
  }

  public void RecordSideEffect(string sideEffect)
  {
    SideEffects = string.IsNullOrEmpty(SideEffects) ? sideEffect : $"{SideEffects}, {sideEffect}";
  }
}

public enum MedicationStatus
{
  Active = 1,
  Stopped = 2,
  Completed = 3,
  OnHold = 4
}
