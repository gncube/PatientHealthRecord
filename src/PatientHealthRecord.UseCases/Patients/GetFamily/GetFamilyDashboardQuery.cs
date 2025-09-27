using PatientHealthRecord.UseCases.Patients;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.GetFamily;

/// <summary>
/// Get family dashboard data for a patient.
/// </summary>
public record GetFamilyDashboardQuery(Guid FamilyId) : Ardalis.SharedKernel.IQuery<Result<List<PatientSummaryDto>>>;
