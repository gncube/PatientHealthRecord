using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.UseCases.Patients.Create;

public class CreatePatientHandler : ICommandHandler<CreatePatientCommand, Result<PatientDto>>
{
  private readonly IRepository<Patient> _repository;

  public CreatePatientHandler(IRepository<Patient> repository)
  {
    _repository = repository;
  }

  public async Task<Result<PatientDto>> Handle(CreatePatientCommand request,
    CancellationToken cancellationToken)
  {
    var patient = new Patient(
      request.Email,
      request.FirstName,
      request.LastName,
      request.DateOfBirth,
      request.Gender,
      request.PhoneNumber
    );

    var createdPatient = await _repository.AddAsync(patient, cancellationToken);

    return Result.Success(new PatientDTO(
      createdPatient.PatientId.Value,
      createdPatient.Email,
      createdPatient.FirstName,
      createdPatient.LastName,
      createdPatient.DateOfBirth,
      createdPatient.Gender.ToString(),
      createdPatient.PhoneNumber,
      createdPatient.IsActive,
      createdPatient.CreatedAt
    ));
  }
}
