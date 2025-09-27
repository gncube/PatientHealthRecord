using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.Interfaces;

namespace PatientHealthRecord.UseCases.Patients.GetFamily;

public class GetFamilyDashboardHandler : IQueryHandler<GetFamilyDashboardQuery, Result<List<PatientSummaryDto>>>
{
    private readonly IPatientRepository _repository;

    public GetFamilyDashboardHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<PatientSummaryDto>>> Handle(GetFamilyDashboardQuery request,
      CancellationToken cancellationToken)
    {
        var patients = await _repository.GetFamilyDashboardAsync(request.FamilyId, cancellationToken);

        var familyDashboard = patients.Select(p => new PatientSummaryDto(
          p.PatientId.Value,
          p.FullName,
          p.Age,
          p.Relationship ?? "Self",
          p.LastAccessedAt ?? p.CreatedAt
        )).ToList();

        return Result.Success(familyDashboard);
    }
}
