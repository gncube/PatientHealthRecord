namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Request to stop a medication
/// </summary>
public class StopMedicationRequest
{
    /// <summary>
    /// The date when the medication was stopped
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Optional reason for stopping the medication
    /// </summary>
    public string? Reason { get; set; }
}
