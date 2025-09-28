using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.Complete;

/// <summary>
/// Command to complete a medication course
/// </summary>
public record CompleteMedicationCommand(
    int Id,
    DateTime? CompletionDate = null) : IRequest<Result>;
