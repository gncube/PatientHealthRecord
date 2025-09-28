using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.RecordSideEffect;

/// <summary>
/// Command to record a side effect for a medication
/// </summary>
public record RecordSideEffectCommand(
    int Id,
    string SideEffect) : IRequest<Result>;

/// <summary>
/// Handler for the RecordSideEffectCommand
/// </summary>
public class RecordSideEffectCommandHandler(
    IRepository<Medication> repository) : IRequestHandler<RecordSideEffectCommand, Result>
{
    public async Task<Result> Handle(RecordSideEffectCommand request, CancellationToken cancellationToken)
    {
        var existingMedication = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingMedication is null)
        {
            return Result.NotFound($"Medication with ID {request.Id} not found.");
        }

        existingMedication.RecordSideEffect(request.SideEffect);

        await repository.UpdateAsync(existingMedication, cancellationToken);

        return Result.Success();
    }
}
