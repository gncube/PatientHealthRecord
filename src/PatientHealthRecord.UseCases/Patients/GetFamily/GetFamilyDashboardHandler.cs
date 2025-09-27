using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.UseCases.Patients;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.GetFamily;

/// <summary>
/// Handler for getting family dashboard data for a patient.
/// </summary>
public class GetFamilyDashboardHandler : IRequestHandler<GetFamilyDashboardQuery, Result<List<PatientSummaryDto>>>
{
    private readonly IPatientRepository _patientRepository;

    public GetFamilyDashboardHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<List<PatientSummaryDto>>> Handle(GetFamilyDashboardQuery request, CancellationToken cancellationToken)
    {
        // Check if the primary patient exists
        var primaryPatient = await _patientRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (primaryPatient is null)
        {
            return Result.NotFound();
        }

        // Get all family members
        var familyMembers = await _patientRepository.GetFamilyDashboardAsync(request.FamilyId, cancellationToken);

        var familyMemberDtos = familyMembers
            .Select(p => new PatientSummaryDto(
                p.PatientId.Value,
                p.FullName,
                p.Age,
                p.Relationship ?? "Unknown",
                p.LastAccessedAt ?? p.CreatedAt
            ))
            .OrderBy(p => p.Relationship == "Self" ? 0 : 1) // Self first, then others
            .ThenBy(p => p.Age) // Then by age
            .ToList();

        return Result.Success(familyMemberDtos);
    }
}
