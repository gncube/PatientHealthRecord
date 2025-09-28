using PatientHealthRecord.UseCases.ClinicalObservations.List;

namespace PatientHealthRecord.Web.ClinicalData;

/// <summary>
/// List all Clinical Observations
/// </summary>
public class ListClinicalObservations(IMediator _mediator) : EndpointWithoutRequest<ClinicalObservationListResponse>
{
    public override void Configure()
    {
        Get("/ClinicalObservations");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ListClinicalObservationsQuery(), cancellationToken);

        if (result.IsSuccess)
        {
            Response = new ClinicalObservationListResponse
            {
                ClinicalObservations = result.Value.Select(co => new ClinicalObservationRecord(
                  co.Id,
                  co.PatientId.Value,
                  co.ObservationType,
                  co.Value,
                  co.Unit,
                  co.RecordedAt,
                  co.RecordedBy,
                  co.Category.ToString(),
                  co.Notes,
                  co.IsVisibleToFamily)).ToList()
            };
        }
    }
}
