using Ardalis.Result;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications;

public interface IListMedicationsQueryService
{
    Task<Result<List<Medication>>> ListAsync(Guid? patientId = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
}
