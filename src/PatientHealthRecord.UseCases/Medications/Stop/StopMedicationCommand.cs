using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.Stop;

/// <summary>
/// Command to stop a medication
/// </summary>
public record StopMedicationCommand(
    int Id,
    DateTime? EndDate = null,
    string? Reason = null) : IRequest<Result>;
