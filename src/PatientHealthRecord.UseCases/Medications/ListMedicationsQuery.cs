using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications;

/// <summary>
/// Query to list medications with optional filtering
/// </summary>
public record ListMedicationsQuery(
    Guid? PatientId = null,
    int? Skip = null,
    int? Take = null) : IRequest<Result<List<Medication>>>;

/// <summary>
/// Handler for the ListMedicationsQuery
/// </summary>
public class ListMedicationsQueryHandler(IListMedicationsQueryService queryService)
    : IRequestHandler<ListMedicationsQuery, Result<List<Medication>>>
{
    public async Task<Result<List<Medication>>> Handle(
        ListMedicationsQuery request,
        CancellationToken cancellationToken)
    {
        return await queryService.ListAsync(request.PatientId, request.Skip, request.Take, cancellationToken);
    }
}
