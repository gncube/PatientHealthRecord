using Ardalis.Result;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;

namespace PatientHealthRecord.UseCases.Interoperability.ExportPatientData;

/// <summary>
/// Export patient data in FHIR format.
/// </summary>
public record ExportPatientDataQuery(
    PatientId PatientId,
    FhirExportFormat Format = FhirExportFormat.Json,
    bool IncludeObservations = true,
    bool IncludeConditions = true,
    bool IncludeMedications = true,
    DateTime? FromDate = null,
    DateTime? ToDate = null
) : Ardalis.SharedKernel.IQuery<Result<FhirExportDto>>;

public enum FhirExportFormat
{
  Json,
  Xml
}
