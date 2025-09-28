using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.Complete;

/// <summary>
/// Command to complete a medication course
/// </summary>
public record CompleteMedicationCommand(
    int Id,
    DateTime? EndDate = null) : IRequest<Result>;

/// <summary>
/// Handler for the CompleteMedicationCommand
/// </summary>
public class CompleteMedicationCommandHandler(
    IRepository<Medication> repository) : IRequestHandler<CompleteMedicationCommand, Result>
{
    public async Task<Result> Handle(CompleteMedicationCommand request, CancellationToken cancellationToken)
    {
        var existingMedication = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingMedication is null)
        {
            return Result.NotFound($"Medication with ID {request.Id} not found.");
        }

        existingMedication.Complete(request.EndDate);

        await repository.UpdateAsync(existingMedication, cancellationToken);

        return Result.Success();
    }
}
