using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.UseCases.ClinicalObservations.Update;

/// <summary>
/// Command to update a clinical observation
/// </summary>
public record UpdateClinicalObservationCommand(
    int Id,
    Guid PatientId,
    string ObservationType,
    string Value,
    string? Unit,
    DateTime RecordedAt,
    string RecordedBy,
    string Category,
    string? Notes,
    bool IsVisibleToFamily) : IRequest<Result>;

/// <summary>
/// Handler for the UpdateClinicalObservationCommand
/// </summary>
public class UpdateClinicalObservationCommandHandler(
    IRepository<ClinicalObservation> repository) : IRequestHandler<UpdateClinicalObservationCommand, Result>
{
    public async Task<Result> Handle(UpdateClinicalObservationCommand request, CancellationToken cancellationToken)
    {
        var existingObservation = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingObservation is null)
        {
            return Result.NotFound($"Clinical observation with ID {request.Id} not found.");
        }

        // Validate that the category is valid
        if (!Enum.TryParse<ObservationCategory>(request.Category, true, out var category))
        {
            return Result.Error($"Invalid category: {request.Category}. Valid categories are: {string.Join(", ", Enum.GetNames<ObservationCategory>())}");
        }

        // Since ClinicalObservation is immutable, we need to create a new instance
        // In a real application, you might want to add update methods to the domain model
        // For now, we'll update the existing entity by creating a new one with the same ID
        var updatedObservation = new ClinicalObservation(
            new PatientId(request.PatientId),
            request.ObservationType,
            request.Value,
            request.Unit,
            request.RecordedAt,
            request.RecordedBy,
            category,
            request.Notes,
            request.IsVisibleToFamily);

        // Copy the ID to maintain the same entity
        var entityBase = updatedObservation as EntityBase;
        if (entityBase != null)
        {
            // This is a bit of a hack since the domain model doesn't expose ID setting
            // In a real application, you'd modify the domain model to support updates
            typeof(EntityBase).GetProperty("Id")?.SetValue(entityBase, request.Id);
        }

        await repository.UpdateAsync(updatedObservation, cancellationToken);

        return Result.Success();
    }
}
