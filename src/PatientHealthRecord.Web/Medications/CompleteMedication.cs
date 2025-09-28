using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.Medications.Complete;

namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Complete a medication
/// </summary>
public class CompleteMedication(IMediator _mediator) : Endpoint<CompleteMedicationRequest>
{
    public override void Configure()
    {
        Post("/Medications/{Id}/complete");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Complete a medication";
            s.Description = "Marks a medication as completed";
        });
    }

    public override async Task HandleAsync(CompleteMedicationRequest request, CancellationToken cancellationToken)
    {
        var id = Route<int>("Id");
        var command = new CompleteMedicationCommand(id, request.CompletionDate);
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
