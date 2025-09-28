using Microsoft.Extensions.Logging;

namespace PatientHealthRecord.UseCases.Contributors.List;

public class ListContributorsHandler(IListContributorsQueryService _query, ILogger<ListContributorsHandler> _logger)
  : IQueryHandler<ListContributorsQuery, Result<IEnumerable<ContributorDTO>>>
{
  public async Task<Result<IEnumerable<ContributorDTO>>> Handle(ListContributorsQuery request, CancellationToken cancellationToken)
  {
    var startTime = DateTime.UtcNow;
    _logger.LogInformation("Starting ListContributorsHandler at {StartTime}", startTime);

    var result = await _query.ListAsync();

    var endTime = DateTime.UtcNow;
    var duration = endTime - startTime;
    _logger.LogInformation("Completed ListContributorsHandler in {Duration}ms", duration.TotalMilliseconds);

    return Result.Success(result);
  }
}
