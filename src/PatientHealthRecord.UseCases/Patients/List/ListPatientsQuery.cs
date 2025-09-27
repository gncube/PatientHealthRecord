using Ardalis.SharedKernel;
using PatientHealthRecord.UseCases.Patients;

namespace PatientHealthRecord.UseCases.Patients.List;

public record ListPatientsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<PatientDto>>>;

public class ListPatientsQueryHandler : IQueryHandler<ListPatientsQuery, Result<IEnumerable<PatientDto>>>
{
    private readonly IListPatientsQueryService _query;

    public ListPatientsQueryHandler(IListPatientsQueryService query)
    {
        _query = query;
    }

    public async Task<Result<IEnumerable<PatientDto>>> Handle(ListPatientsQuery request, CancellationToken cancellationToken)
    {
        var result = await _query.ListAsync();

        Console.WriteLine($"<<<<<<<Listed {result.Count()} patients");

        return Result.Success(result);
    }
}
