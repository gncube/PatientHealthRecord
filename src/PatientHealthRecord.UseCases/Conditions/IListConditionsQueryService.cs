using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions;

public interface IListConditionsQueryService
{
    Task<IEnumerable<Condition>> ListAsync(Guid? patientId = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
}