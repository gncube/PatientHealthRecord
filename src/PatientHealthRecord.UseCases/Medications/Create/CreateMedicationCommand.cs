using Ardalis.Result;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.UseCases.Medications.Create;

public record CreateMedicationCommand(
    Guid PatientId,
    string Name,
    string? Dosage = null,
    string? Frequency = null,
    string? Instructions = null,
    DateTime? StartDate = null,
    string? PrescribedBy = null,
    string? Purpose = null,
    string RecordedBy = "Self",
    bool IsVisibleToFamily = true) : ICommand<Result<int>>;
