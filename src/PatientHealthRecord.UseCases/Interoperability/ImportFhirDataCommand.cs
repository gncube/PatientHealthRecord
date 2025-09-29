using Ardalis.Result;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;

namespace PatientHealthRecord.UseCases.Interoperability;

public record ImportFhirDataCommand(
    PatientId PatientId,
    string FhirContent,
    FhirImportFormat Format,
    bool ValidateResources = true,
    bool OverwriteExisting = false
) : Ardalis.SharedKernel.ICommand<Result<FhirImportResultDto>>;

public enum FhirImportFormat
{
  Json,
  Xml
}
