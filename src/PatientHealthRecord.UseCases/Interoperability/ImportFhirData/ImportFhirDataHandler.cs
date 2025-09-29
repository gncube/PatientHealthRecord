using Ardalis.Result;
using Ardalis.SharedKernel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using PatientHealthRecord.Core.InteroperabilityAggregate;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;
using System.Threading.Tasks;

namespace PatientHealthRecord.UseCases.Interoperability.ImportFhirData;

/// <summary>
/// Handler for importing FHIR data.
/// </summary>
public class ImportFhirDataHandler : ICommandHandler<ImportFhirDataCommand, Result<FhirImportResultDto>>
{
  private readonly IRepository<FhirResource> _fhirRepository;
  private readonly IRepository<ClinicalObservation> _observationRepository;
  private readonly IRepository<PatientHealthRecord.Core.ClinicalDataAggregate.Condition> _conditionRepository;
  private readonly IRepository<PatientHealthRecord.Core.ClinicalDataAggregate.Medication> _medicationRepository;

  public ImportFhirDataHandler(
      IRepository<FhirResource> fhirRepository,
      IRepository<ClinicalObservation> observationRepository,
      IRepository<PatientHealthRecord.Core.ClinicalDataAggregate.Condition> conditionRepository,
      IRepository<PatientHealthRecord.Core.ClinicalDataAggregate.Medication> medicationRepository)
  {
    _fhirRepository = fhirRepository;
    _observationRepository = observationRepository;
    _conditionRepository = conditionRepository;
    _medicationRepository = medicationRepository;
  }

  public async Task<Result<FhirImportResultDto>> Handle(ImportFhirDataCommand request, CancellationToken cancellationToken)
  {
    try
    {
      // Parse FHIR content
      Resource resource;
      if (request.Format == FhirImportFormat.Xml)
      {
        var parser = new FhirXmlParser();
        resource = parser.Parse<Resource>(request.FhirContent);
      }
      else
      {
        var parser = new FhirJsonParser();
        resource = parser.Parse<Resource>(request.FhirContent);
      }

      var importResult = new FhirImportResultDto();

      if (resource is Bundle bundle)
      {
        await ProcessBundle(bundle, request.PatientId, importResult, cancellationToken);
      }
      else
      {
        await ProcessSingleResource(resource, request.PatientId, importResult, cancellationToken);
      }

      return Result.Success(importResult);
    }
    catch (Exception ex)
    {
      return Result.Error($"Failed to import FHIR data: {ex.Message}");
    }
  }

  private async System.Threading.Tasks.Task ProcessBundle(Bundle bundle, PatientId patientId, FhirImportResultDto result, CancellationToken cancellationToken)
  {
    foreach (var entry in bundle.Entry)
    {
      if (entry.Resource != null)
      {
        await ProcessSingleResource(entry.Resource, patientId, result, cancellationToken);
      }
    }
  }

  private async System.Threading.Tasks.Task ProcessSingleResource(Resource resource, PatientId patientId, FhirImportResultDto result, CancellationToken cancellationToken)
  {
    var serializer = new FhirJsonSerializer();
    var content = serializer.SerializeToString(resource);

    // Store as FHIR resource
    var fhirResource = new FhirResource(resource.TypeName, resource.Id ?? Guid.NewGuid().ToString(), patientId, content, "Import");
    await _fhirRepository.AddAsync(fhirResource, cancellationToken);

    result.ImportedResources++;

    // Convert specific resources to internal entities
    try
    {
      switch (resource)
      {
        case Observation obs:
          await ConvertAndStoreObservation(obs, patientId, cancellationToken);
          result.ConvertedObservations++;
          break;

        case Hl7.Fhir.Model.Condition cond:
          await ConvertAndStoreCondition(cond, patientId, cancellationToken);
          result.ConvertedConditions++;
          break;

        case MedicationRequest medReq:
          await ConvertAndStoreMedication(medReq, patientId, cancellationToken);
          result.ConvertedMedications++;
          break;
      }
    }
    catch (Exception ex)
    {
      result.Errors.Add($"Failed to convert {resource.TypeName}: {ex.Message}");
    }
  }

  private async System.Threading.Tasks.Task ConvertAndStoreObservation(Observation fhirObs, PatientId patientId, CancellationToken cancellationToken)
  {
    var observationType = fhirObs.Code?.Text ?? fhirObs.Code?.Coding?.FirstOrDefault()?.Display ?? "Unknown";
    var value = ExtractObservationValue(fhirObs);
    var unit = ExtractObservationUnit(fhirObs);
    var recordedAt = DateTime.UtcNow; // Simplified for now
    if (fhirObs.Effective is FhirDateTime effectiveDateTime)
    {
      recordedAt = effectiveDateTime.ToDateTimeOffset(TimeSpan.Zero).DateTime;
    }
    var performer = fhirObs.Performer?.FirstOrDefault()?.Display ?? "Import";

    var observation = new ClinicalObservation(
        patientId, observationType, value, unit, recordedAt, performer,
        ObservationCategory.General,
        fhirObs.Note?.FirstOrDefault()?.Text,
        true);

    await _observationRepository.AddAsync(observation, cancellationToken);
  }

  private async System.Threading.Tasks.Task ConvertAndStoreCondition(Hl7.Fhir.Model.Condition fhirCond, PatientId patientId, CancellationToken cancellationToken)
  {
    var name = fhirCond.Code?.Text ?? fhirCond.Code?.Coding?.FirstOrDefault()?.Display ?? "Unknown Condition";
    var description = fhirCond.Note?.FirstOrDefault()?.Text;
    var onsetDate = (DateTime?)null;
    if (fhirCond.Onset is FhirDateTime onsetDateTime)
    {
      onsetDate = onsetDateTime.ToDateTimeOffset(TimeSpan.Zero).DateTime;
    }

    var severity = fhirCond.Severity?.Coding?.FirstOrDefault()?.Code switch
    {
      "24484000" => ConditionSeverity.Severe,
      "6736007" => ConditionSeverity.Moderate,
      _ => ConditionSeverity.Mild
    };

    var condition = new PatientHealthRecord.Core.ClinicalDataAggregate.Condition(
        patientId, name, description, onsetDate, severity, null, "Import", true);

    if (fhirCond.ClinicalStatus?.Coding?.FirstOrDefault()?.Code == "resolved")
    {
      condition.Resolve();
    }

    await _conditionRepository.AddAsync(condition, cancellationToken);
  }

  private async System.Threading.Tasks.Task ConvertAndStoreMedication(MedicationRequest fhirMedReq, PatientId patientId, CancellationToken cancellationToken)
  {
    var name = ExtractMedicationName(fhirMedReq);
    var dosage = fhirMedReq.DosageInstruction?.FirstOrDefault()?.Text;
    var startDate = (DateTime?)null;
    if (fhirMedReq.AuthoredOnElement is FhirDateTime authoredDateTime)
    {
      startDate = authoredDateTime.ToDateTimeOffset(TimeSpan.Zero).DateTime;
    }
    var prescriber = fhirMedReq.Requester?.Display;
    var purpose = fhirMedReq.ReasonCode?.FirstOrDefault()?.Text;

    var medication = new PatientHealthRecord.Core.ClinicalDataAggregate.Medication(
        patientId, name, dosage, null, null, startDate, prescriber, purpose, "Import", true);

    if (fhirMedReq.Status == MedicationRequest.MedicationrequestStatus.Stopped)
    {
      medication.Stop();
    }

    await _medicationRepository.AddAsync(medication, cancellationToken);
  }

  private string ExtractObservationValue(Observation obs)
  {
    return obs.Value switch
    {
      Quantity quantity => quantity.Value?.ToString() ?? "",
      FhirString str => str.Value ?? "",
      FhirBoolean boolean => boolean.Value?.ToString() ?? "",
      _ => obs.Component?.FirstOrDefault()?.Value?.ToString() ?? ""
    };
  }

  private string? ExtractObservationUnit(Observation obs)
  {
    return obs.Value switch
    {
      Quantity quantity => quantity.Unit,
      _ => obs.Component?.FirstOrDefault()?.Value is Quantity compQuantity ? compQuantity.Unit : null
    };
  }

  private string ExtractMedicationName(MedicationRequest medReq)
  {
    return medReq.Medication switch
    {
      CodeableConcept concept => concept.Text ?? concept.Coding?.FirstOrDefault()?.Display ?? "Unknown Medication",
      ResourceReference reference => reference.Display ?? "Unknown Medication",
      _ => "Unknown Medication"
    };
  }
}
