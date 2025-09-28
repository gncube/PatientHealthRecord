using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.RecordSideEffect;

/// <summary>
/// Handler for recording a side effect for a medication
/// </summary>
public class RecordSideEffectCommandHandler : IRequestHandler<RecordSideEffectCommand, Result>
{
    private readonly IRepository<Medication> _repository;

    public RecordSideEffectCommandHandler(IRepository<Medication> repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(RecordSideEffectCommand request, CancellationToken cancellationToken)
    {
        var medication = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (medication == null)
        {
            return Result.NotFound($"Medication with ID {request.Id} not found");
        }

        var result = medication.RecordSideEffect(request.SideEffect, request.Severity, request.ReportedDate ?? DateTime.UtcNow);
        if (result.IsSuccess)
        {
            await _repository.UpdateAsync(medication, cancellationToken);
            return Result.Success();
        }
        else
        {
            return Result.Error(result.Errors.FirstOrDefault() ?? "Failed to record side effect");
        }
    }
}
