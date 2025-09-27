using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.Interfaces;

namespace PatientHealthRecord.UseCases.Patients.Get;

public class GetPatientHandler : IQueryHandler<GetPatientQuery, Result<PatientDto>>
{
  private readonly IPatientRepository _repository;

  public GetPatientHandler(IPatientRepository repository)
  {
    _repository = repository;
  }

  public async Task<Result<PatientDto>> Handle(GetPatientQuery request,
    CancellationToken cancellationToken)
  {
    var patient = await _repository.GetByIdAsync(request.PatientId, cancellationToken);

    if (patient == null)
    {
      return Result.NotFound($"Patient with ID {request.PatientId} not found.");
    }

    return Result.Success(new PatientDto(
      patient.PatientId.Value,
      patient.Email,
      patient.FirstName,
      patient.LastName,
      patient.DateOfBirth,
      patient.Gender.ToString(),
      patient.PhoneNumber,
      patient.Relationship,
      patient.EmergencyContactName,
      patient.EmergencyContactPhone,
      patient.EmergencyContactRelationship,
      patient.BloodType,
      patient.Allergies,
      patient.Notes,
      patient.ShareWithFamily,
      patient.RestrictedDataTypes,
      patient.IsActive,
      patient.CreatedAt,
      patient.LastAccessedAt
    ));
  }
}
