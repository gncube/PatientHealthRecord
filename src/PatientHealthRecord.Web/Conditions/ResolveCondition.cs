using FastEndpoints;
using PatientHealthRecord.UseCases.Conditions;

namespace PatientHealthRecord.Web.Conditions;

/// <summary>
/// Resolve a condition
/// </summary>
public class ResolveCondition(IMediator _mediator) : Endpoint<ResolveConditionRequest>
{
    public override void Configure()
    {
        Post("/conditions/{Id}/resolve");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ResolveConditionRequest request, CancellationToken cancellationToken)
    {
        var id = Route<int>("Id");
        var command = new ResolveConditionCommand(id, request.ResolutionNotes);
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
