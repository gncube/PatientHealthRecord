using FastEndpoints;
using PatientHealthRecord.UseCases.Conditions;

namespace PatientHealthRecord.Web.Conditions;

/// <summary>
/// Delete a condition
/// </summary>
public class DeleteCondition(IMediator _mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/conditions/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<int>("Id");
        var command = new DeleteConditionCommand(id);
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
