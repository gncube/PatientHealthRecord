using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.ClinicalObservations.List;

/// <summary>
/// Represents a service that will actually fetch the necessary data for clinical observations
/// Typically implemented in Infrastructure
/// </summary>
public interface IListClinicalObservationsQueryService
{
    Task<IEnumerable<ClinicalObservation>> ListAsync(Guid? patientId = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
}
