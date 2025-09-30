using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions.Delete;

public record DeleteConditionCommand(int Id) : ICommand<Result>;

public class DeleteConditionCommandHandler(IRepository<Condition> _repository) : ICommandHandler<DeleteConditionCommand, Result>
{
  public async Task<Result> Handle(DeleteConditionCommand request, CancellationToken cancellationToken)
  {
    var condition = await _repository.GetByIdAsync(request.Id, cancellationToken);
    if (condition == null)
    {
      return Result.NotFound($"Condition with ID {request.Id} not found");
    }

    await _repository.DeleteAsync(condition, cancellationToken);
    return Result.Success();
  }
}
