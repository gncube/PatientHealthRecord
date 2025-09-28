using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.RecordSideEffect;

/// <summary>
/// Command to record a side effect for a medication
/// </summary>
public record RecordSideEffectCommand(
    int Id,
    string SideEffect,
    string Severity = "Mild",
    DateTime? ReportedDate = null) : IRequest<Result>;
