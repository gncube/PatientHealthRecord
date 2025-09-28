using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.Complete;

/// <summary>
/// Handler for completing a medication
/// </summary>
public class CompleteMedicationCommandHandler : IRequestHandler<CompleteMedicationCommand, Result>
{
    private readonly IRepository<Medication> _repository;

    public CompleteMedicationCommandHandler(IRepository<Medication> repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(CompleteMedicationCommand request, CancellationToken cancellationToken)
    {
        var medication = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (medication == null)
        {
            return Result.NotFound($"Medication with ID {request.Id} not found");
        }

        var result = medication.Complete(request.CompletionDate);
        if (result.IsSuccess)
        {
            await _repository.UpdateAsync(medication, cancellationToken);
            return Result.Success();
        }
        else
        {
            return Result.Error(result.Errors.ToArray());
        }
    }
}
