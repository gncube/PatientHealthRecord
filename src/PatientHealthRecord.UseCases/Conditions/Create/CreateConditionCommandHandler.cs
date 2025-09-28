using Ardalis.Result;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.UseCases.Conditions.Create;

public class CreateConditionCommandHandler(IRepository<Condition> _repository, IPatientRepository _patientRepository) : ICommandHandler<CreateConditionCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateConditionCommand request, CancellationToken cancellationToken)
    {
        // Validate that patient exists
        var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);
        if (patient == null)
        {
            return Result.NotFound($"Patient with ID {request.PatientId} not found");
        }

        // Parse severity enum
        if (!Enum.TryParse<ConditionSeverity>(request.Severity, true, out var severity))
        {
            severity = ConditionSeverity.Mild;
        }

        var condition = new Condition(
            patientId: new PatientId(request.PatientId),
            name: request.Name,
            description: request.Description,
            onsetDate: request.OnsetDate,
            severity: severity,
            treatment: request.Treatment,
            recordedBy: request.RecordedBy);

        var createdCondition = await _repository.AddAsync(condition, cancellationToken);
        return Result.Success(createdCondition.Id);
    }
}
