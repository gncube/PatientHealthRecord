using Microsoft.Extensions.Options;
using PatientHealthRecord.Core.Options;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.PatientAggregate.Specifications;

namespace PatientHealthRecord.UseCases.Patients.Create;

public class CreatePatientHandler : ICommandHandler<CreatePatientCommand, Result<PatientDto>>
{
  private readonly IRepository<Patient> _repository;
  private readonly FamilyPlanOptions _familyOptions;

  public CreatePatientHandler(IRepository<Patient> repository, IOptions<FamilyPlanOptions> familyOptions)
  {
    _repository = repository;
    _familyOptions = familyOptions.Value;
  }

  public async Task<Result<PatientDto>> Handle(CreatePatientCommand request,
    CancellationToken cancellationToken)
  {
    // Check family plan limits
    var existingPatients = await _repository.CountAsync(cancellationToken);
    if (existingPatients >= _familyOptions.MaxPatients)
    {
      return Result.Error($"Maximum number of patients ({_familyOptions.MaxPatients}) reached for family plan.");
    }

    // Check if email already exists
    var existingPatient = await _repository.FirstOrDefaultAsync(
      new PatientByEmailSpec(request.Email), cancellationToken);

    if (existingPatient != null)
    {
      return Result.Error($"Patient with email {request.Email} already exists.");
    }

    var patient = new Patient(
      request.Email,
      request.FirstName,
      request.LastName,
      request.DateOfBirth,
      request.Gender,
      request.Relationship,
      request.PrimaryContactId,
      request.PhoneNumber
    );

    // Add emergency contact if provided
    if (!string.IsNullOrEmpty(request.EmergencyContactName) &&
        !string.IsNullOrEmpty(request.EmergencyContactPhone))
    {
      patient.UpdateEmergencyContact(request.EmergencyContactName,
        request.EmergencyContactPhone, "Emergency Contact");
    }

    // Add medical information if provided
    if (!string.IsNullOrEmpty(request.BloodType) || request.Allergies?.Any() == true)
    {
      patient.UpdateMedicalInfo(request.BloodType, request.Allergies, null);
    }

    var createdPatient = await _repository.AddAsync(patient, cancellationToken);

    return Result.Success(new PatientDto(
      createdPatient.PatientId.Value,
      createdPatient.Email,
      createdPatient.FirstName,
      createdPatient.LastName,
      createdPatient.DateOfBirth,
      createdPatient.Gender.ToString(),
      createdPatient.Relationship,
      createdPatient.PhoneNumber,
      createdPatient.BloodType,
      createdPatient.Allergies,
      createdPatient.IsActive,
      createdPatient.CreatedAt
    ));
  }
}
