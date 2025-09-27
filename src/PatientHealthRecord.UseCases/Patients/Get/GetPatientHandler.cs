using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.UseCases.Patients;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.Get;

/// <summary>
/// Handler for getting a Patient by ID.
/// </summary>
public class GetPatientHandler : IRequestHandler<GetPatientQuery, Result<PatientDto>>
{
  private readonly IPatientRepository _patientRepository;

  public GetPatientHandler(IPatientRepository patientRepository)
  {
    _patientRepository = patientRepository;
  }

  public async Task<Result<PatientDto>> Handle(GetPatientQuery request, CancellationToken cancellationToken)
  {
    var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);

    if (patient is null)
    {
      return Result.NotFound();
    }

    var patientDto = new PatientDto(
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
    );

    return Result.Success(patientDto);
  }
}
