using FastEndpoints;
using PatientHealthRecord.UseCases.Conditions.GetById;

namespace PatientHealthRecord.Web.Conditions;

/// <summary>
/// Get a specific condition by ID
/// </summary>
public class GetCondition(IMediator _mediator) : EndpointWithoutRequest<ConditionRecord>
{
    public override void Configure()
    {
        Get("/conditions/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<int>("Id");
        var query = new GetConditionByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        var response = new ConditionRecord(
            Id: result.Id,
            PatientId: result.PatientId.Value,
            Name: result.Name,
            Description: result.Description,
            OnsetDate: result.OnsetDate,
            Severity: result.Severity.ToString(),
            Status: result.Status.ToString(),
            Treatment: result.Treatment,
            ResolvedDate: result.ResolvedDate,
            RecordedBy: result.RecordedBy,
            RecordedAt: result.RecordedAt,
            IsVisibleToFamily: result.IsVisibleToFamily);

        await SendAsync(response, cancellation: cancellationToken);
    }
}
