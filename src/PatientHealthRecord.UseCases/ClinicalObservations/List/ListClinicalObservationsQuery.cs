using Ardalis.SharedKernel;
using PatientHealthRecord.UseCases.ClinicalObservations;

namespace PatientHealthRecord.UseCases.ClinicalObservations.List;

public record ListClinicalObservationsQuery(Guid PatientId, int? Skip, int? Take) : IQuery<Result<IEnumerable<ClinicalObservationDto>>>;
