using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.Interfaces;
using Ardalis.Result;

namespace PatientHealthRecord.UseCases.Conditions.List;

public class ListConditionsQueryService : IListConditionsQueryService
{
    private readonly IRepository<Condition> _repository;

    public ListConditionsQueryService(IRepository<Condition> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<Condition>>> ListAsync(Guid? patientId = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        var spec = new ListConditionsSpecification(patientId);

        IEnumerable<Condition> conditions = await _repository.ListAsync(spec, cancellationToken);

        // Apply skip/take if provided
        if (skip.HasValue)
        {
            conditions = conditions.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            conditions = conditions.Take(take.Value);
        }

        return Result.Success(conditions.ToList());
    }
}
