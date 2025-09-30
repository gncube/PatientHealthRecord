using Ardalis.Result;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions.List;

public interface IListConditionsQueryService
{
    Task<Result<List<Condition>>> ListAsync(Guid? patientId = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
}
