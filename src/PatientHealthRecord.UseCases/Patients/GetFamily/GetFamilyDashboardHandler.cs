using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.PatientAggregate.Specifications;
using PatientHealthRecord.UseCases.Patients;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.GetFamily;

/// <summary>
/// Handler for getting family dashboard data for a patient.
/// </summary>
public class GetFamilyDashboardHandler : IRequestHandler<GetFamilyDashboardQuery, Result<List<PatientSummaryDto>>>
{
    private readonly IRepository<Patient> _patientRepository;

    public GetFamilyDashboardHandler(IRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<List<PatientSummaryDto>>> Handle(GetFamilyDashboardQuery request, CancellationToken cancellationToken)
    {
        // Check if the primary patient exists
        var primaryPatientSpec = new PatientByIdSpec(request.FamilyId);
        var primaryPatient = await _patientRepository.FirstOrDefaultAsync(primaryPatientSpec, cancellationToken);
        if (primaryPatient is null)
        {
            return Result.NotFound();
        }

        // Get all family members for dashboard
        var familyDashboardSpec = new FamilyDashboardSpec(request.FamilyId);
        var familyMembers = await _patientRepository.ListAsync(familyDashboardSpec, cancellationToken);

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
