using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.UseCases.Patients.Get;

public class GetPatientHandler : IQueryHandler<GetPatientQuery, Result<PatientDto>>
{
  private readonly IReadRepository<Patient> _repository;

  public GetPatientHandler(IReadRepository<Patient> repository)
  {
    _repository = repository;
  }

  public async Task<Result<PatientDto>> Handle(GetPatientQuery request,
    CancellationToken cancellationToken)
  {
    var patientId = new PatientId(request.PatientId);
    var patient = await _repository.GetByIdAsync(patientId.Value, cancellationToken);

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
      patient.Relationship,
      patient.PhoneNumber,
      patient.BloodType,
      patient.Allergies,
      patient.IsActive,
      patient.CreatedAt
    ));
  }
}
