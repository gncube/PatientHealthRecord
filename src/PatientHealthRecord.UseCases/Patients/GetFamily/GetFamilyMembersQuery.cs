using Ardalis.Result;

namespace PatientHealthRecord.UseCases.Patients.GetFamily;

public record GetFamilyMembersQuery(
  Guid? PrimaryContactId = null
) : Ardalis.SharedKernel.IQuery<Result<List<PatientSummaryDto>>>;
