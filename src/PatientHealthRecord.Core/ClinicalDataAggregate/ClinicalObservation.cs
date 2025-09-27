using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Core.ClinicalDataAggregate;

public class ClinicalObservation : EntityBase
{
  public PatientId PatientId { get; private set; }
  public string ObservationType { get; private set; } = string.Empty; // Weight, Height, Blood Pressure, etc.
  public string Value { get; private set; } = string.Empty;
  public string? Unit { get; private set; }
  public DateTime RecordedAt { get; private set; }
  public string? Notes { get; private set; }
  public string RecordedBy { get; private set; } = string.Empty; // Family member name or "Self"

  // Simple categorization for family use
  public ObservationCategory Category { get; private set; }
  public bool IsVisibleToFamily { get; private set; } = true;

  private ClinicalObservation()
  {
    PatientId = new PatientId(Guid.Empty);
  }

  public ClinicalObservation(PatientId patientId, string observationType,
    string value, string? unit, DateTime recordedAt, string recordedBy,
    ObservationCategory category = ObservationCategory.General,
    string? notes = null, bool isVisibleToFamily = true)
  {
    PatientId = Guard.Against.Null(patientId, nameof(patientId));
    ObservationType = Guard.Against.NullOrEmpty(observationType, nameof(observationType));
    Value = Guard.Against.NullOrEmpty(value, nameof(value));
    Unit = unit;
    RecordedAt = recordedAt;
    RecordedBy = Guard.Against.NullOrEmpty(recordedBy, nameof(recordedBy));
    Category = category;
    Notes = notes;
    IsVisibleToFamily = isVisibleToFamily;
  }

  public void UpdateVisibility(bool isVisibleToFamily)
  {
    IsVisibleToFamily = isVisibleToFamily;
  }

  public void AddNotes(string notes)
  {
    Notes = string.IsNullOrEmpty(Notes) ? notes : $"{Notes}\n{notes}";
  }

  // Helper methods for common observations
  public static ClinicalObservation Weight(PatientId patientId, decimal weightKg,
    DateTime recordedAt, string recordedBy, string? notes = null)
  {
    return new ClinicalObservation(patientId, "Weight", weightKg.ToString("F1"),
      "kg", recordedAt, recordedBy, ObservationCategory.Vital, notes);
  }

  public static ClinicalObservation Height(PatientId patientId, decimal heightCm,
    DateTime recordedAt, string recordedBy, string? notes = null)
  {
    return new ClinicalObservation(patientId, "Height", heightCm.ToString("F1"),
      "cm", recordedAt, recordedBy, ObservationCategory.Vital, notes);
  }

  public static ClinicalObservation BloodPressure(PatientId patientId, int systolic, int diastolic,
    DateTime recordedAt, string recordedBy, string? notes = null)
  {
    return new ClinicalObservation(patientId, "Blood Pressure", $"{systolic}/{diastolic}",
      "mmHg", recordedAt, recordedBy, ObservationCategory.Vital, notes);
  }
}
