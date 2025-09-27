using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;

namespace PatientHealthRecord.UseCases.Patients.Create;

public class CreatePatientHandler : ICommandHandler<CreatePatientCommand, Result<Guid>>
{
  private readonly IRepository<Patient> _repository;

  public CreatePatientHandler(IRepository<Patient> repository)
  {
    _repository = repository;
  }

  public async Task<Result<Guid>> Handle(CreatePatientCommand request,
    CancellationToken cancellationToken)
  {
    var gender = Enum.Parse<Gender>(request.Gender);
    var newPatient = new Patient(
      request.Email,
      request.FirstName,
      request.LastName,
      request.DateOfBirth,
      gender,
      request.Relationship ?? "Self",
      request.PrimaryContactId,
      request.PhoneNumber
    );
    var createdItem = await _repository.AddAsync(newPatient, cancellationToken);

    return createdItem.PatientId.Value;
  }
}
