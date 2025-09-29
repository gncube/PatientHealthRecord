using Ardalis.SharedKernel;
using PatientHealthRecord.Core.ContributorAggregate;
using PatientHealthRecord.UseCases.Contributors.Create;
using FastEndpoints;

namespace PatientHealthRecord.UseCases.Contributors.List;

public record ListContributorsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ContributorDto>>>;
public record ListContributorsQuery2(int? Skip, int? Take) : FastEndpoints.ICommand<Result<IEnumerable<ContributorDto>>>;

public class ListContributorsQueryHandler2 : CommandHandler<ListContributorsQuery2, Result<IEnumerable<ContributorDto>>>
{
  private readonly IListContributorsQueryService _query;

  public ListContributorsQueryHandler2(IListContributorsQueryService query)
  {
    _query = query;
  }
  public override async Task<Result<IEnumerable<ContributorDto>>> ExecuteAsync(ListContributorsQuery2 request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    Console.WriteLine($"<<<<<<<Listed {result.Count()} contributors");

    return Result.Success(result);
  }
}
