using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.ClinicalObservations.Delete;

namespace PatientHealthRecord.Web.ClinicalData;

/// <summary>
/// Endpoint for deleting a clinical observation
/// </summary>
public class DeleteClinicalObservation : Endpoint<DeleteClinicalObservationRequest, DeleteClinicalObservationResponse>
{
    private readonly IMediator _mediator;

    public DeleteClinicalObservation(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/ClinicalObservations/{Id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Delete a clinical observation";
            s.Description = "Deletes a clinical observation by its ID";
        });
    }

    public override async Task HandleAsync(DeleteClinicalObservationRequest req, CancellationToken ct)
    {
        var command = new DeleteClinicalObservationCommand(req.Id);
        var result = await _mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            var response = new DeleteClinicalObservationResponse
            {
                Message = "Clinical observation deleted successfully"
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
/// Request DTO for deleting a clinical observation
/// </summary>
public class DeleteClinicalObservationRequest
{
    public int Id { get; set; }
}

/// <summary>
/// Response DTO for deleting a clinical observation
/// </summary>
public class DeleteClinicalObservationResponse
{
    public string Message { get; set; } = string.Empty;
}
