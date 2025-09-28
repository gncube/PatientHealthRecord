using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.Stop;

/// <summary>
/// Handler for stopping a medication
/// </summary>
public class StopMedicationCommandHandler : IRequestHandler<StopMedicationCommand, Result>
{
    private readonly IRepository<Medication> _repository;

    public StopMedicationCommandHandler(IRepository<Medication> repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(StopMedicationCommand request, CancellationToken cancellationToken)
    {
        var medication = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (medication == null)
        {
            return Result.NotFound($"Medication with ID {request.Id} not found");
        }

        var result = medication.Stop(request.EndDate, request.Reason);
        if (result.IsSuccess)
        {
            await _repository.UpdateAsync(medication, cancellationToken);
            return Result.Success();
        }
        else
        {
            return Result.Error(result.Errors.FirstOrDefault() ?? "Failed to stop medication");
        }
    }
}
