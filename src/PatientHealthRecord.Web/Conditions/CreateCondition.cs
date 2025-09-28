using FastEndpoints;
using PatientHealthRecord.UseCases.Conditions;

namespace PatientHealthRecord.Web.Conditions;

/// <summary>
/// Create a new condition
/// </summary>
public class CreateCondition(IMediator _mediator) : Endpoint<CreateConditionRequest, int>
{
    public override void Configure()
    {
        Post("/conditions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateConditionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateConditionCommand(
            request.PatientId,
            request.Name,
            request.Description,
            request.OnsetDate,
            request.Severity,
            request.Treatment,
            request.RecordedBy);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            await SendCreatedAtAsync<GetCondition>(new { Id = result.Value }, result.Value, cancellation: cancellationToken);
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
