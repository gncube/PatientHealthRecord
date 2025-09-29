namespace PatientHealthRecord.Core.Options;

public class FeatureOptions
{
    public bool EnableFhirExport { get; set; } = false;
    public bool EnableAuditLogging { get; set; } = false;
    public bool EnableAdvancedSearch { get; set; } = false;
    public bool EnableApiAccess { get; set; } = false;
}
