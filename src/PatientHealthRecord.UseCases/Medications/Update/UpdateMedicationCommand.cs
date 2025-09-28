using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.UseCases.Medications.Update;

/// <summary>
/// Command to update a medication
/// </summary>
public record UpdateMedicationCommand(
    int Id,
    Guid PatientId,
    string Name,
    string? Dosage,
    string? Frequency,
    string? Instructions,
    DateTime StartDate,
    string? PrescribedBy,
    string? Purpose,
    string RecordedBy,
    bool IsVisibleToFamily) : IRequest<Result>;

/// <summary>
/// Handler for the UpdateMedicationCommand
/// </summary>
public class UpdateMedicationCommandHandler(
    IRepository<Medication> repository) : IRequestHandler<UpdateMedicationCommand, Result>
{
    public async Task<Result> Handle(UpdateMedicationCommand request, CancellationToken cancellationToken)
    {
        var existingMedication = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingMedication is null)
        {
            return Result.NotFound($"Medication with ID {request.Id} not found.");
        }

        // Since Medication is immutable, we need to create a new instance
        var updatedMedication = new Medication(
            new PatientId(request.PatientId),
            request.Name,
            request.Dosage,
            request.Frequency,
            request.Instructions,
            request.StartDate,
            request.PrescribedBy,
            request.Purpose,
            request.RecordedBy,
            request.IsVisibleToFamily);

        // Copy the ID to maintain the same entity
        var entityBase = updatedMedication as EntityBase;
        if (entityBase != null)
        {
            typeof(EntityBase).GetProperty("Id")?.SetValue(entityBase, request.Id);
        }

        await repository.UpdateAsync(updatedMedication, cancellationToken);

        return Result.Success();
    }
}
