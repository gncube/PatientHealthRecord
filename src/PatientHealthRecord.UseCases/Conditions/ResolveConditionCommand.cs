using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions;

public record ResolveConditionCommand(int Id, string ResolutionNotes) : ICommand<Result>;

public class ResolveConditionCommandHandler(IRepository<Condition> _repository) : ICommandHandler<ResolveConditionCommand, Result>
{
    public async Task<Result> Handle(ResolveConditionCommand request, CancellationToken cancellationToken)
    {
        var condition = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (condition == null)
        {
            return Result.NotFound($"Condition with ID {request.Id} not found");
        }

        condition.Resolve();
        await _repository.UpdateAsync(condition, cancellationToken);
        return Result.Success();
    }
}
