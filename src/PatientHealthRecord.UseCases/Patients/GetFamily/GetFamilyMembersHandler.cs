using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.PatientAggregate.Specifications;
using PatientHealthRecord.UseCases.Patients;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.GetFamily;

/// <summary>
/// Handler for getting family members for a patient.
/// </summary>
public class GetFamilyMembersHandler : IRequestHandler<GetFamilyMembersQuery, Result<List<PatientSummaryDto>>>
{
  private readonly IRepository<Patient> _patientRepository;

  public GetFamilyMembersHandler(IRepository<Patient> patientRepository)
  {
    _patientRepository = patientRepository;
  }

  public async Task<Result<List<PatientSummaryDto>>> Handle(GetFamilyMembersQuery request, CancellationToken cancellationToken)
  {
    // Check if the primary patient exists
    var primaryPatientSpec = new PatientByIdSpec(request.FamilyId);
    var primaryPatient = await _patientRepository.FirstOrDefaultAsync(primaryPatientSpec, cancellationToken);
    if (primaryPatient is null)
    {
      return Result.NotFound();
    }

    // Get all family members
    var familyMembersSpec = new FamilyMembersSpec(request.FamilyId);
    var familyMembers = await _patientRepository.ListAsync(familyMembersSpec, cancellationToken);

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
