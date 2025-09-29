using PatientHealthRecord.UseCases.Interoperability;

namespace PatientHealthRecord.Web.Interoperability;

/// <summary>
/// Request to export patient data in FHIR format
/// </summary>
public class ExportPatientDataRequest
{
    /// <summary>
    /// The patient ID to export data for
    /// </summary>
    public Guid PatientId { get; set; }

    /// <summary>
    /// Export format (Json or Xml)
    /// </summary>
    public string Format { get; set; } = "Json";

    /// <summary>
    /// Include clinical observations in export
    /// </summary>
    public bool IncludeObservations { get; set; } = true;

    /// <summary>
    /// Include conditions in export
    /// </summary>
    public bool IncludeConditions { get; set; } = true;

    /// <summary>
    /// Include medications in export
    /// </summary>
    public bool IncludeMedications { get; set; } = true;

    /// <summary>
    /// Start date for filtering clinical data (optional)
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// End date for filtering clinical data (optional)
    /// </summary>
    public DateTime? ToDate { get; set; }

    public const string Route = "/Interoperability/ExportPatientData";
}
