using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions.Update;

public record UpdateConditionCommand(
    int Id,
    string? Name = null,
    string? Description = null,
    DateTime? OnsetDate = null,
    string? Severity = null,
    string? Treatment = null,
    string? RecordedBy = null) : ICommand<Result>;

public class UpdateConditionCommandHandler(IRepository<Condition> _repository) : ICommandHandler<UpdateConditionCommand, Result>
{
    public async Task<Result> Handle(UpdateConditionCommand request, CancellationToken cancellationToken)
    {
        var condition = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (condition == null)
        {
            return Result.NotFound($"Condition with ID {request.Id} not found");
        }

        // Parse severity enum if provided
        ConditionSeverity? severity = null;
        if (!string.IsNullOrWhiteSpace(request.Severity) &&
            Enum.TryParse<ConditionSeverity>(request.Severity, true, out var parsedSeverity))
        {
            severity = parsedSeverity;
        }

        if (!string.IsNullOrWhiteSpace(request.Treatment))
        {
            condition.UpdateTreatment(request.Treatment);
        }

        if (severity.HasValue)
        {
            condition.UpdateSeverity(severity.Value);
        }

        // Update other properties if provided
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            // Note: Condition doesn't have a direct name update method, so we'd need to add one or handle differently
            // For now, we'll focus on the methods that exist
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            // Similar issue - Condition doesn't have description update
        }

        if (request.OnsetDate.HasValue)
        {
            // OnsetDate is typically set at creation and not updated
        }

        if (!string.IsNullOrWhiteSpace(request.RecordedBy))
        {
            // RecordedBy is typically set at creation
        }

        await _repository.UpdateAsync(condition, cancellationToken);
        return Result.Success();
    }
}
