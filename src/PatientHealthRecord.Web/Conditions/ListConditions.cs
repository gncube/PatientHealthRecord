using FastEndpoints;
using PatientHealthRecord.UseCases.Conditions.List;

namespace PatientHealthRecord.Web.Conditions;

/// <summary>
/// Request for listing conditions
/// </summary>
public class ConditionsRequest
{
    /// <summary>
    /// Patient ID to filter conditions (optional)
    /// </summary>
    public Guid? PatientId { get; set; }

    /// <summary>
    /// Number of records to skip
    /// </summary>
    public int? Skip { get; set; }

    /// <summary>
    /// Maximum number of records to return
    /// </summary>
    public int? Take { get; set; }
}

/// <summary>
/// List all Conditions
/// </summary>
public class ListConditions(IMediator _mediator) : Endpoint<ConditionsRequest, ConditionListResponse>
{
    public override void Configure()
    {
        Get("/conditions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ConditionsRequest request, CancellationToken cancellationToken)
    {
        var query = new ListConditionsQuery(request.PatientId, request.Skip, request.Take);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            Response = new ConditionListResponse
            {
                Conditions = result.Value.Select(c => new ConditionRecord(
                    Id: c.Id,
                    PatientId: c.PatientId.Value,
                    Name: c.Name,
                    Description: c.Description,
                    OnsetDate: c.OnsetDate,
                    Severity: c.Severity.ToString(),
                    Status: c.Status.ToString(),
                    Treatment: c.Treatment,
                    ResolvedDate: c.ResolvedDate,
                    RecordedBy: c.RecordedBy,
                    RecordedAt: c.RecordedAt,
                    IsVisibleToFamily: c.IsVisibleToFamily)).ToList()
            };
        }
    }
}
