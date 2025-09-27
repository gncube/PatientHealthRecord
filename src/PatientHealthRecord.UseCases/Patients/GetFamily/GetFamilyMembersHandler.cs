using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.UseCases.Patients;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.GetFamily;

/// <summary>
/// Handler for getting family members for a patient.
/// </summary>
public class GetFamilyMembersHandler : IRequestHandler<GetFamilyMembersQuery, Result<List<PatientSummaryDto>>>
{
  private readonly IPatientRepository _patientRepository;

  public GetFamilyMembersHandler(IPatientRepository patientRepository)
  {
    _patientRepository = patientRepository;
  }

  public async Task<Result<List<PatientSummaryDto>>> Handle(GetFamilyMembersQuery request, CancellationToken cancellationToken)
  {
    // Check if the primary patient exists
    var primaryPatient = await _patientRepository.GetByIdAsync(request.FamilyId, cancellationToken);
    if (primaryPatient is null)
    {
      return Result.NotFound();
    }

    // Get all family members
    var familyMembers = await _patientRepository.GetFamilyMembersAsync(request.FamilyId, cancellationToken);

    var familyMemberDtos = familyMembers
        .Select(p => new PatientSummaryDto(
            p.PatientId.Value,
            p.FullName,
            p.Age,
            p.Relationship ?? "Unknown",
            p.LastAccessedAt ?? p.CreatedAt
        ))
        .ToList();

    return Result.Success(familyMemberDtos);
  }
}
