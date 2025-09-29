using Ardalis.Result;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;

namespace PatientHealthRecord.UseCases.Interoperability;

public record ExportPatientDataCommand(
    PatientId PatientId,
    FhirExportFormat Format = FhirExportFormat.Json,
    bool IncludeObservations = true,
    bool IncludeConditions = true,
    bool IncludeMedications = true,
    DateTime? FromDate = null,
    DateTime? ToDate = null
) : Ardalis.SharedKernel.ICommand<Result<FhirExportDto>>;

public enum FhirExportFormat
{
  Json,
  Xml
}
