namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Request to complete a medication
/// </summary>
public class CompleteMedicationRequest
{
    /// <summary>
    /// The date when the medication was completed
    /// </summary>
    public DateTime CompletionDate { get; set; }
}
