using FastEndpoints;
using PatientHealthRecord.UseCases.Conditions;

namespace PatientHealthRecord.Web.Conditions;

/// <summary>
/// Update an existing condition
/// </summary>
public class UpdateCondition(IMediator _mediator) : Endpoint<UpdateConditionRequest>
{
    public override void Configure()
    {
        Put("/conditions/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateConditionRequest request, CancellationToken cancellationToken)
    {
        var id = Route<int>("Id");
        var command = new UpdateConditionCommand(
            id,
            request.Name,
            request.Description,
            request.OnsetDate,
            request.Severity,
            request.Treatment,
            request.RecordedBy);

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
