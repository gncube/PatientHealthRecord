using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.ClinicalObservations.Update;

namespace PatientHealthRecord.Web.ClinicalData;

/// <summary>
/// Endpoint for updating a clinical observation
/// </summary>
public class UpdateClinicalObservation : Endpoint<UpdateClinicalObservationRequest, UpdateClinicalObservationResponse>
{
    private readonly IMediator _mediator;

    public UpdateClinicalObservation(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/ClinicalObservations/{Id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Update a clinical observation";
            s.Description = "Updates an existing clinical observation by its ID";
            s.ExampleRequest = new UpdateClinicalObservationRequest
            {
                Id = 1,
                PatientId = Guid.NewGuid(),
                ObservationType = "Weight",
                Value = "72.0",
                Unit = "kg",
                RecordedAt = DateTime.UtcNow,
                RecordedBy = "Dr. Smith",
                Category = "Vital",
                Notes = "Updated weight",
                IsVisibleToFamily = true
            };
        });
    }

    public override async Task HandleAsync(UpdateClinicalObservationRequest req, CancellationToken ct)
    {
        var command = new UpdateClinicalObservationCommand(
            req.Id,
            req.PatientId,
            req.ObservationType,
            req.Value,
            req.Unit,
            req.RecordedAt,
            req.RecordedBy,
            req.Category,
            req.Notes,
            req.IsVisibleToFamily);

        var result = await _mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            var response = new UpdateClinicalObservationResponse
            {
                Message = "Clinical observation updated successfully"
            };
            await SendAsync(response, cancellation: ct);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                AddError(error);
            }
            await SendErrorsAsync(cancellation: ct);
        }
    }
}

/// <summary>
/// Request DTO for updating a clinical observation
/// </summary>
public class UpdateClinicalObservationRequest
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
    public bool IsVisibleToFamily { get; set; } = true;
}

/// <summary>
/// Response DTO for updating a clinical observation
/// </summary>
public class UpdateClinicalObservationResponse
{
    public string Message { get; set; } = string.Empty;
}
