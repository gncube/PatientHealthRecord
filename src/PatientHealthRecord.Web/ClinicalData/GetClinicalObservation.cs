using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.ClinicalObservations.Get;

namespace PatientHealthRecord.Web.ClinicalData;

/// <summary>
/// Endpoint for getting a clinical observation by ID
/// </summary>
public class GetClinicalObservation : Endpoint<GetClinicalObservationRequest, GetClinicalObservationResponse>
{
    private readonly IMediator _mediator;

    public GetClinicalObservation(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/ClinicalObservations/{Id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get a clinical observation by ID";
            s.Description = "Retrieves a specific clinical observation by its ID";
        });
    }

    public override async Task HandleAsync(GetClinicalObservationRequest req, CancellationToken ct)
    {
        var query = new GetClinicalObservationByIdQuery(req.Id);
        var result = await _mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            var observation = result.Value;
            var response = new GetClinicalObservationResponse
            {
                Id = observation.Id,
                PatientId = observation.PatientId.Value,
                ObservationType = observation.ObservationType,
                Value = observation.Value,
                Unit = observation.Unit,
                RecordedAt = observation.RecordedAt,
                RecordedBy = observation.RecordedBy,
                Category = observation.Category.ToString(),
                Notes = observation.Notes,
                IsVisibleToFamily = observation.IsVisibleToFamily
            };
            await SendAsync(response, cancellation: ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}

/// <summary>
/// Request DTO for getting a clinical observation by ID
/// </summary>
public class GetClinicalObservationRequest
{
    public int Id { get; set; }
}

/// <summary>
/// Response DTO for getting a clinical observation by ID
/// </summary>
public class GetClinicalObservationResponse
{
    public int Id { get; set; }
    public Guid PatientId { get; set; }
    public string ObservationType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Unit { get; set; }
    public DateTime RecordedAt { get; set; }
    public string RecordedBy { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsVisibleToFamily { get; set; }
}
