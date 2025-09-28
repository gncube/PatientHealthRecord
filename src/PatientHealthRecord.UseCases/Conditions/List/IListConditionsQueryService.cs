using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions.List;

/// <summary>
/// Represents a service that will actually fetch the necessary data for conditions
/// Typically implemented in Infrastructure
/// </summary>
public interface IListConditionsQueryService
{
    Task<IEnumerable<Condition>> ListAsync(Guid? patientId = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
}
