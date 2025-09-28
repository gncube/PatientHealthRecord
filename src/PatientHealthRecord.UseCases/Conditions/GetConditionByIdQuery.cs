using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions;

public record GetConditionByIdQuery(int ConditionId) : IQuery<Condition?>;

public class GetConditionByIdQueryHandler(IRepository<Condition> _repository) : IQueryHandler<GetConditionByIdQuery, Condition?>
{
    public async Task<Condition?> Handle(GetConditionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.ConditionId, cancellationToken);
    }
}
