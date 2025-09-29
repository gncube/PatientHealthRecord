namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

/// <summary>
/// Specification for finding a patient by their PatientId (strongly-typed ID).
/// </summary>
public class PatientByIdSpec : Specification<Patient>
{
  public PatientByIdSpec(PatientId patientId) =>
    Query
        .Where(patient => patient.PatientId == patientId);

  public PatientByIdSpec(Guid patientId) =>
    Query
        .Where(patient => patient.PatientId == new PatientId(patientId));
}
