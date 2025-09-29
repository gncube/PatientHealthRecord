namespace PatientHealthRecord.Core.Options;

public class FamilyPlanOptions
{
  public int MaxPatients { get; set; } = 10;
  public int MaxObservationsPerPatient { get; set; } = 1000;
  public int MaxConditionsPerPatient { get; set; } = 50;
  public int MaxMedicationsPerPatient { get; set; } = 100;
  public bool EnableDataExport { get; set; } = true;
  public bool EnableFamilySharing { get; set; } = true;
}
