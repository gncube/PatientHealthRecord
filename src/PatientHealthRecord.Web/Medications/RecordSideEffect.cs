using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.Medications.RecordSideEffect;

namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Record a side effect for a medication
/// </summary>
public class RecordSideEffect(IMediator _mediator) : Endpoint<RecordSideEffectRequest>
{
    public override void Configure()
    {
        Post("/Medications/{Id}/side-effect");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Record a side effect";
            s.Description = "Records a side effect for a medication";
        });
    }

    public override async Task HandleAsync(RecordSideEffectRequest request, CancellationToken cancellationToken)
    {
        var id = Route<int>("Id");
        var command = new RecordSideEffectCommand(id, request.SideEffect, request.Severity, request.ReportedDate);
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
