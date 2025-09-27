using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.Create;

/// <summary>
/// Handler for creating a new Patient.
/// </summary>
public class CreatePatientHandler : IRequestHandler<CreatePatientCommand, Result<Guid>>
{
  private readonly IPatientRepository _patientRepository;

  public CreatePatientHandler(IPatientRepository patientRepository)
  {
    _patientRepository = patientRepository;
  }

  public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
  {
    if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
    {
      return Result.Error("Invalid gender value");
    }

    var patient = new Patient(
        request.Email,
        request.FirstName,
        request.LastName,
        request.DateOfBirth,
        gender,
        request.Relationship ?? "Self",
        request.PrimaryContactId,
        request.PhoneNumber
    );

    var createdPatient = await _patientRepository.AddAsync(patient, cancellationToken);

    return Result.Success(createdPatient.PatientId.Value);
  }
}
