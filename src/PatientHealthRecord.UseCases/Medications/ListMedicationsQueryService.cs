using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.Interfaces;
using Ardalis.Result;

namespace PatientHealthRecord.UseCases.Medications;

public class ListMedicationsQueryService : IListMedicationsQueryService
{
    private readonly IRepository<Medication> _repository;

    public ListMedicationsQueryService(IRepository<Medication> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<Medication>>> ListAsync(Guid? patientId = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        var spec = new ListMedicationsSpecification(patientId);

        IEnumerable<Medication> medications = await _repository.ListAsync(spec, cancellationToken);

        // Apply skip/take if provided
        if (skip.HasValue)
        {
            medications = medications.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            medications = medications.Take(take.Value);
        }

        return Result.Success(medications.ToList());
    }
}
