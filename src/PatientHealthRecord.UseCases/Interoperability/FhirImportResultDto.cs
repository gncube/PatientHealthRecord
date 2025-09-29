namespace PatientHealthRecord.UseCases.Interoperability;

public class FhirImportResultDto
{
    public int ImportedResources { get; set; }
    public int ConvertedObservations { get; set; }
    public int ConvertedConditions { get; set; }
    public int ConvertedMedications { get; set; }
    public List<string> Errors { get; set; } = new();
    public DateTimeOffset ImportedAt { get; set; } = DateTimeOffset.UtcNow;
}
