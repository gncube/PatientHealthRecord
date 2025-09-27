namespace PatientHealthRecord.UseCases.Patients.List;

public class ListPatientsHandler(IListPatientsQueryService _query)
  : IQueryHandler<ListPatientsQuery, Result<IEnumerable<PatientDto>>>
{
    public async Task<Result<IEnumerable<PatientDto>>> Handle(ListPatientsQuery request, CancellationToken cancellationToken)
    {
        var result = await _query.ListAsync();

        return Result.Success(result);
    }
}
