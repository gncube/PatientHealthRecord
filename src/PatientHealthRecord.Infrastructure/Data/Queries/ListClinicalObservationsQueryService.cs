using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.UseCases.ClinicalObservations.List;

namespace PatientHealthRecord.Infrastructure.Data.Queries;

public class ListClinicalObservationsQueryService(AppDbContext _db) : IListClinicalObservationsQueryService
{
    public async Task<IEnumerable<ClinicalObservation>> ListAsync(Guid? patientId = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        var query = _db.ClinicalObservations.AsQueryable();

        // Filter by PatientId if provided
        if (patientId.HasValue)
        {
            var patientGuid = patientId.Value;
            query = query.Where(co => co.PatientId.Value == patientGuid);
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

        return await query.ToListAsync(cancellationToken);
    }
}
