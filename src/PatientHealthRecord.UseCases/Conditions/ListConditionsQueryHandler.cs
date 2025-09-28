using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions;

public class ListConditionsQueryHandler(IListConditionsQueryService _queryService) : IQueryHandler<ListConditionsQuery, IEnumerable<Condition>>
{
    public async Task<IEnumerable<Condition>> Handle(ListConditionsQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.ListAsync(request.PatientId, request.Skip, request.Take, cancellationToken);
    }
}
