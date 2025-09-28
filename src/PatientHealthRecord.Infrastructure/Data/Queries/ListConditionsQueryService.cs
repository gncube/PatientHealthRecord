using Ardalis.Result;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.UseCases.Conditions;

namespace PatientHealthRecord.Infrastructure.Data.Queries;

public class ListConditionsQueryService(AppDbContext _db) : IListConditionsQueryService
{
    public async Task<Result<List<Condition>>> ListAsync(Guid? patientId = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        var query = _db.Conditions.AsQueryable();

        // Filter by PatientId if provided
        if (patientId.HasValue)
        {
            var patientGuid = patientId.Value;
            query = query.Where(c => c.PatientId.Value == patientGuid);
        }

        // Apply pagination
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        var conditions = await query.ToListAsync(cancellationToken);
        return Result.Success(conditions);
    }
}
