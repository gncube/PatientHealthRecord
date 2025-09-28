using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.Medications.Delete;

namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Endpoint for deleting a medication
/// </summary>
public class DeleteMedication : Endpoint<DeleteMedicationRequest, DeleteMedicationResponse>
{
    private readonly IMediator _mediator;

    public DeleteMedication(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Delete("/Medications/{Id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Delete a medication";
            s.Description = "Deletes a medication by its ID";
        });
    }

    public override async Task HandleAsync(DeleteMedicationRequest req, CancellationToken ct)
    {
        var command = new DeleteMedicationCommand(req.Id);
        var result = await _mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            var response = new DeleteMedicationResponse
            {
                Message = "Medication deleted successfully"
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
/// Request DTO for deleting a medication
/// </summary>
public class DeleteMedicationRequest
{
    public int Id { get; set; }
}

/// <summary>
/// Response DTO for deleting a medication
/// </summary>
public class DeleteMedicationResponse
{
    public string Message { get; set; } = string.Empty;
}
