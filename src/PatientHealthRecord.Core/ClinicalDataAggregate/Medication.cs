using Ardalis.Result;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Core.ClinicalDataAggregate;

public class Medication : EntityBase, IAggregateRoot
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
    string? frequency = null, string? instructions = null, DateTime? startDate = null, string? prescribedBy = null,
    string? purpose = null, string recordedBy = "Self", bool isVisibleToFamily = true)
  {
    PatientId = Guard.Against.Null(patientId, nameof(patientId));
    Name = Guard.Against.NullOrEmpty(name, nameof(name));
    Dosage = dosage;
    Frequency = frequency;
    Instructions = instructions;
    StartDate = startDate ?? DateTime.UtcNow;
    Status = MedicationStatus.Active;
    PrescribedBy = prescribedBy;
    Purpose = purpose;
    RecordedBy = recordedBy;
    IsVisibleToFamily = isVisibleToFamily;
  }

  public Result Stop(DateTime? endDate = null, string? reason = null)
  {
    if (Status != MedicationStatus.Active)
    {
      return Result.Error("Cannot stop a medication that is not active");
    }
    Status = MedicationStatus.Stopped;
    EndDate = endDate ?? DateTime.UtcNow;
    if (!string.IsNullOrEmpty(reason))
    {
      Instructions = string.IsNullOrEmpty(Instructions) ? reason : $"{Instructions}\nStopped: {reason}";
    }
    return Result.Success();
  }

  public Result Complete(DateTime? completionDate = null)
  {
    if (Status != MedicationStatus.Active)
    {
      return Result.Error("Cannot complete a medication that is not active");
    }
    Status = MedicationStatus.Completed;
    EndDate = completionDate ?? DateTime.UtcNow;
    return Result.Success();
  }

  public Result RecordSideEffect(string sideEffect, string severity, DateTime reportedDate)
  {
    if (string.IsNullOrEmpty(sideEffect))
    {
      return Result.Error("Side effect description is required");
    }
    var sideEffectEntry = $"{reportedDate:yyyy-MM-dd}: {sideEffect} (Severity: {severity})";
    SideEffects = string.IsNullOrEmpty(SideEffects) ? sideEffectEntry : $"{SideEffects}\n{sideEffectEntry}";
    return Result.Success();
  }
}

public enum MedicationStatus
{
  Active = 1,
  Stopped = 2,
  Completed = 3,
  OnHold = 4
}
