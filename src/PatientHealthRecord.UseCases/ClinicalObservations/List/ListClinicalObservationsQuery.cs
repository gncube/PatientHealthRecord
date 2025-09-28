using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.ClinicalObservations.List;

/// <summary>
/// Query to list clinical observations with optional filtering
/// </summary>
public record ListClinicalObservationsQuery(
    Guid? PatientId = null,
    int? Skip = null,
    int? Take = null) : IRequest<Result<List<ClinicalObservation>>>;

/// <summary>
/// Handler for the ListClinicalObservationsQuery
/// </summary>
public class ListClinicalObservationsQueryHandler(IListClinicalObservationsQueryService queryService)
    : IRequestHandler<ListClinicalObservationsQuery, Result<List<ClinicalObservation>>>
{
    public async Task<Result<List<ClinicalObservation>>> Handle(
        ListClinicalObservationsQuery request,
        CancellationToken cancellationToken)
    {
        var observations = await queryService.ListAsync(request.PatientId, request.Skip, request.Take, cancellationToken);
        return Result.Success(observations.ToList());
    }
}
