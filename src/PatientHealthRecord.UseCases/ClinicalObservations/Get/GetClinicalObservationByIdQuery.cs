using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.ClinicalObservations.Get;

/// <summary>
/// Query to get a clinical observation by ID
/// </summary>
public record GetClinicalObservationByIdQuery(int Id) : IRequest<Result<ClinicalObservation>>;

/// <summary>
/// Handler for the GetClinicalObservationByIdQuery
/// </summary>
public class GetClinicalObservationByIdQueryHandler(
    IRepository<ClinicalObservation> repository) : IRequestHandler<GetClinicalObservationByIdQuery, Result<ClinicalObservation>>
{
    public async Task<Result<ClinicalObservation>> Handle(GetClinicalObservationByIdQuery request, CancellationToken cancellationToken)
    {
        var observation = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (observation is null)
        {
            return Result.NotFound($"Clinical observation with ID {request.Id} not found.");
        }

        return Result.Success(observation);
    }
}
