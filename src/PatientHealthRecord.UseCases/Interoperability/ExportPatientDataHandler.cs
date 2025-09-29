using PatientHealthRecord.Core.Services;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.InteroperabilityAggregate;
using PatientHealthRecord.Core.Specifications;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace PatientHealthRecord.UseCases.Interoperability;

public class ExportPatientDataHandler : ICommandHandler<ExportPatientDataCommand, Result<FhirExportDto>>
{
  private readonly IReadRepository<PatientHealthRecord.Core.PatientAggregate.Patient> _patientRepository;
  private readonly IReadRepository<PatientHealthRecord.Core.ClinicalDataAggregate.ClinicalObservation> _observationRepository;
  private readonly IReadRepository<PatientHealthRecord.Core.ClinicalDataAggregate.Condition> _conditionRepository;
  private readonly IReadRepository<PatientHealthRecord.Core.ClinicalDataAggregate.Medication> _medicationRepository;
  private readonly IRepository<PatientHealthRecord.Core.InteroperabilityAggregate.FhirResource> _fhirRepository;
  private readonly IFhirConversionService _fhirConversionService;

  public ExportPatientDataHandler(
      IReadRepository<PatientHealthRecord.Core.PatientAggregate.Patient> patientRepository,
      IReadRepository<PatientHealthRecord.Core.ClinicalDataAggregate.ClinicalObservation> observationRepository,
      IReadRepository<PatientHealthRecord.Core.ClinicalDataAggregate.Condition> conditionRepository,
      IReadRepository<PatientHealthRecord.Core.ClinicalDataAggregate.Medication> medicationRepository,
      IRepository<PatientHealthRecord.Core.InteroperabilityAggregate.FhirResource> fhirRepository,
      IFhirConversionService fhirConversionService)
  {
    _patientRepository = patientRepository;
    _observationRepository = observationRepository;
    _conditionRepository = conditionRepository;
    _medicationRepository = medicationRepository;
    _fhirRepository = fhirRepository;
    _fhirConversionService = fhirConversionService;
  }

  public async Task<Result<FhirExportDto>> Handle(ExportPatientDataCommand request, CancellationToken cancellationToken)
  {
    // Get patient
    var patient = await _patientRepository.GetByIdAsync(request.PatientId.Value, cancellationToken);
    if (patient == null)
    {
      return Result.NotFound($"Patient with ID {request.PatientId.Value} not found.");
    }

    // Get clinical data based on request
    var observations = new List<PatientHealthRecord.Core.ClinicalDataAggregate.ClinicalObservation>();
    var conditions = new List<PatientHealthRecord.Core.ClinicalDataAggregate.Condition>();
    var medications = new List<PatientHealthRecord.Core.ClinicalDataAggregate.Medication>();

    if (request.IncludeObservations)
    {
      observations = await _observationRepository.ListAsync(
          new ObservationsByPatientAndDateRangeSpec(request.PatientId, request.FromDate, request.ToDate),
          cancellationToken);
    }

    if (request.IncludeConditions)
    {
      conditions = await _conditionRepository.ListAsync(
          new ConditionsByPatientSpec(request.PatientId),
          cancellationToken);
    }

    if (request.IncludeMedications)
    {
      medications = await _medicationRepository.ListAsync(
          new MedicationsByPatientSpec(request.PatientId),
          cancellationToken);
    }

    // Create FHIR Bundle
    var bundle = _fhirConversionService.CreatePatientBundle(patient, observations, conditions, medications);

    // Serialize based on format
    string content;
    string mimeType;

    if (request.Format == FhirExportFormat.Xml)
    {
      var xmlSerializer = new FhirXmlSerializer();
      content = xmlSerializer.SerializeToString(bundle);
      mimeType = "application/fhir+xml";
    }
    else
    {
      var jsonSerializer = new FhirJsonSerializer();
      content = jsonSerializer.SerializeToString(bundle);
      mimeType = "application/fhir+json";
    }

    // Store as FHIR resource for audit trail
    var fhirResource = new PatientHealthRecord.Core.InteroperabilityAggregate.FhirResource("Bundle", bundle.Id, request.PatientId, content, "Family App Export");
    await _fhirRepository.AddAsync(fhirResource, cancellationToken);

    return Result.Success(new FhirExportDto(
        bundle.Id,
        patient.FullName,
        request.Format.ToString(),
        mimeType,
        content,
        bundle.Entry.Count - 1, // Exclude patient resource from count
        DateTime.UtcNow
    ));
  }
}
