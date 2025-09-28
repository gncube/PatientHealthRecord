using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.Delete;

/// <summary>
/// Command to delete a medication
/// </summary>
public record DeleteMedicationCommand(int Id) : IRequest<Result>;

/// <summary>
/// Handler for the DeleteMedicationCommand
/// </summary>
public class DeleteMedicationCommandHandler(
    IRepository<Medication> repository) : IRequestHandler<DeleteMedicationCommand, Result>
{
    public async Task<Result> Handle(DeleteMedicationCommand request, CancellationToken cancellationToken)
    {
        var existingMedication = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingMedication is null)
        {
            return Result.NotFound($"Medication with ID {request.Id} not found.");
        }

        await repository.DeleteAsync(existingMedication, cancellationToken);

        return Result.Success();
    }
}
