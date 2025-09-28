namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Request to record a side effect for a medication
/// </summary>
public class RecordSideEffectRequest
{
    /// <summary>
    /// Description of the side effect
    /// </summary>
    public string SideEffect { get; set; } = string.Empty;

    /// <summary>
    /// Severity level of the side effect
    /// </summary>
    public string Severity { get; set; } = string.Empty;

    /// <summary>
    /// Date when the side effect was reported
    /// </summary>
    public DateTime ReportedDate { get; set; }
}
