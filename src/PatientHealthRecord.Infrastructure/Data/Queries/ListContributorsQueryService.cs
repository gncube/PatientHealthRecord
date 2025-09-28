using Microsoft.Extensions.Logging;
using PatientHealthRecord.UseCases.Contributors;
using PatientHealthRecord.UseCases.Contributors.List;

namespace PatientHealthRecord.Infrastructure.Data.Queries;

public class ListContributorsQueryService(AppDbContext _db, ILogger<ListContributorsQueryService> _logger) : IListContributorsQueryService
{
  // You can use EF, Dapper, SqlClient, etc. for queries -
  // this is just an example

  public async Task<IEnumerable<ContributorDTO>> ListAsync()
  {
    var startTime = DateTime.UtcNow;
    _logger.LogInformation("Starting ListContributorsQueryService.ListAsync at {StartTime}", startTime);

    // NOTE: This will fail if testing with EF InMemory provider!
    var result = await _db.Database.SqlQuery<ContributorDTO>(
      $"SELECT Id, Name, PhoneNumber_Number AS PhoneNumber FROM Contributors") // don't fetch other big columns
      .ToListAsync();

    var endTime = DateTime.UtcNow;
    var duration = endTime - startTime;
    _logger.LogInformation("Completed ListContributorsQueryService.ListAsync in {Duration}ms, returned {Count} contributors", duration.TotalMilliseconds, result.Count);

    return result;
  }
}
