using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.Stop;

/// <summary>
/// Command to stop a medication
/// </summary>
public record StopMedicationCommand(
    int Id,
    DateTime? EndDate = null,
    string? Reason = null) : IRequest<Result>;

/// <summary>
/// Handler for the StopMedicationCommand
/// </summary>
public class StopMedicationCommandHandler(
    IRepository<Medication> repository) : IRequestHandler<StopMedicationCommand, Result>
{
    public async Task<Result> Handle(StopMedicationCommand request, CancellationToken cancellationToken)
    {
        var existingMedication = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingMedication is null)
        {
            return Result.NotFound($"Medication with ID {request.Id} not found.");
        }

        existingMedication.Stop(request.EndDate, request.Reason);

        await repository.UpdateAsync(existingMedication, cancellationToken);

        return Result.Success();
    }
}
