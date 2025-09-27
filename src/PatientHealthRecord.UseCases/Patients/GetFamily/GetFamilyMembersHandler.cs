using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.Interfaces;

namespace PatientHealthRecord.UseCases.Patients.GetFamily;

public class GetFamilyMembersHandler : IQueryHandler<GetFamilyMembersQuery, Result<List<PatientSummaryDto>>>
{
  private readonly IPatientRepository _repository;

  public GetFamilyMembersHandler(IPatientRepository repository)
  {
    _repository = repository;
  }

  public async Task<Result<List<PatientSummaryDto>>> Handle(GetFamilyMembersQuery request,
    CancellationToken cancellationToken)
  {
    var patients = await _repository.GetFamilyMembersAsync(request.FamilyId, cancellationToken);

    var familyMembers = patients.Select(p => new PatientSummaryDto(
      p.PatientId.Value,
      p.FullName,
      p.Age,
      p.Relationship ?? "Self",
      p.LastAccessedAt ?? p.CreatedAt
    )).ToList();

    return Result.Success(familyMembers);
  }
}
