using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;
using PatientHealthRecord.UseCases.Patients;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.Create;

/// <summary>
/// Handler for creating a new patient.
/// </summary>
public class CreatePatientHandler : ICommandHandler<CreatePatientCommand, Result<Guid>>
{
  private readonly IRepository<Patient> _patientRepository;

  public CreatePatientHandler(IRepository<Patient> patientRepository)
  {
    _patientRepository = patientRepository;
  }

  public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
  {
    // Validate email format
    if (!IsValidEmail(request.Email))
    {
      return Result.Error("Invalid email format");
    }

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

  private static bool IsValidEmail(string email)
  {
    if (string.IsNullOrWhiteSpace(email))
      return false;

    try
    {
      var addr = new System.Net.Mail.MailAddress(email);
      return addr.Address == email;
    }
    catch
    {
      return false;
    }
  }
}
