using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions.List;

/// <summary>
/// Query to list conditions with optional filtering
/// </summary>
public record ListConditionsQuery(
    Guid? PatientId = null,
    int? Skip = null,
    int? Take = null) : IRequest<Result<List<Condition>>>;

/// <summary>
/// Handler for the ListConditionsQuery
/// </summary>
public class ListConditionsQueryHandler(IListConditionsQueryService queryService)
    : IRequestHandler<ListConditionsQuery, Result<List<Condition>>>
{
    public async Task<Result<List<Condition>>> Handle(
        ListConditionsQuery request,
        CancellationToken cancellationToken)
    {
        return await queryService.ListAsync(request.PatientId, request.Skip, request.Take, cancellationToken);
    }
}
