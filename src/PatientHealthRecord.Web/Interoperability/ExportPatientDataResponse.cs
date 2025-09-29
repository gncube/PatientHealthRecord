using PatientHealthRecord.UseCases.Interoperability;

namespace PatientHealthRecord.Web.Interoperability;

/// <summary>
/// Response containing exported FHIR patient data
/// </summary>
public class ExportPatientDataResponse
{
    /// <summary>
    /// Unique identifier for the FHIR bundle
    /// </summary>
    public string BundleId { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the patient
    /// </summary>
    public string PatientName { get; set; } = string.Empty;

    /// <summary>
    /// Export format used (Json or Xml)
    /// </summary>
    public string Format { get; set; } = string.Empty;

    /// <summary>
    /// MIME type of the exported content
    /// </summary>
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// The FHIR bundle content as a string
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Number of clinical resources included (excluding patient)
    /// </summary>
    public int ResourceCount { get; set; }

    /// <summary>
    /// When the export was performed
    /// </summary>
    public DateTimeOffset ExportedAt { get; set; }

    public ExportPatientDataResponse() { }

    public ExportPatientDataResponse(FhirExportDto dto)
    {
        BundleId = dto.BundleId;
        PatientName = dto.PatientName;
        Format = dto.Format;
        MimeType = dto.MimeType;
        Content = dto.Content;
        ResourceCount = dto.ResourceCount;
        ExportedAt = dto.ExportedAt;
    }
}
