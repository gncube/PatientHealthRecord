using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.Medications.Stop;

namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Stop a medication
/// </summary>
public class StopMedication(IMediator _mediator) : Endpoint<StopMedicationRequest>
{
    public override void Configure()
    {
        Post("/Medications/{Id}/stop");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Stop a medication";
            s.Description = "Stops a medication and optionally records the reason";
        });
    }

    public override async Task HandleAsync(StopMedicationRequest request, CancellationToken cancellationToken)
    {
        var id = Route<int>("Id");
        var command = new StopMedicationCommand(id, request.EndDate, request.Reason);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await SendNoContentAsync(cancellationToken);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                AddError(error);
            }
            await SendErrorsAsync(cancellation: cancellationToken);
        }
    }
}
