using Microsoft.Extensions.Logging;

namespace PatientHealthRecord.UseCases.Contributors.List;

public class ListContributorsHandler(IListContributorsQueryService _query, ILogger<ListContributorsHandler> _logger)
  : IQueryHandler<ListContributorsQuery, Result<IEnumerable<ContributorDto>>>
{
  public async Task<Result<IEnumerable<ContributorDto>>> Handle(ListContributorsQuery request, CancellationToken cancellationToken)
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
