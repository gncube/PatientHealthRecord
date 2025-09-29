using PatientHealthRecord.UseCases.Contributors;
using PatientHealthRecord.UseCases.Contributors.List;

namespace PatientHealthRecord.Web.Contributors;

/// <summary>
/// List all Contributors
/// </summary>
/// <remarks>
/// List all contributors - returns a ContributorListResponse containing the Contributors.
/// </remarks>
public class List(IMediator _mediator, ILogger<List> _logger) : EndpointWithoutRequest<ContributorListResponse>
{
  public override void Configure()
  {
    Get("/Contributors");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    var startTime = DateTime.UtcNow;
    _logger.LogInformation("Starting List Contributors request at {StartTime}", startTime);

    Result<IEnumerable<ContributorDto>> result = await _mediator.Send(new ListContributorsQuery(null, null), cancellationToken);

    var result2 = await new ListContributorsQuery2(null, null)
      .ExecuteAsync(cancellationToken);

    if (result.IsSuccess)
    {
      Response = new ContributorListResponse
      {
        Contributors = result.Value.Select(c => new ContributorRecord(c.Id, c.Name, c.PhoneNumber)).ToList()
      };
    }

    var endTime = DateTime.UtcNow;
    var duration = endTime - startTime;
    _logger.LogInformation("Completed List Contributors request in {Duration}ms", duration.TotalMilliseconds);
  }
}
