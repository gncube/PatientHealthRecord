using Ardalis.Result;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.PatientAggregate.Specifications;

namespace PatientHealthRecord.UseCases.Conditions.Create;

public class CreateConditionCommandHandler(IRepository<Condition> _repository, IRepository<Patient> _patientRepository) : ICommandHandler<CreateConditionCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateConditionCommand request, CancellationToken cancellationToken)
    {
        // Validate that patient exists
        var patientSpec = new PatientByIdSpec(request.PatientId);
        var patient = await _patientRepository.FirstOrDefaultAsync(patientSpec, cancellationToken);
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
