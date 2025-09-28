using Ardalis.Result;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.UseCases.Conditions.Create;

public record CreateConditionCommand(
    Guid PatientId,
    string Name,
    string? Description = null,
    DateTime? OnsetDate = null,
    string Severity = "Mild",
    string? Treatment = null,
    string RecordedBy = "Self") : ICommand<Result<int>>;
