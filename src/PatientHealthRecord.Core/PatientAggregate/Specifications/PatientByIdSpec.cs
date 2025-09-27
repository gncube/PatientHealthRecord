namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

public class PatientByIdSpec : Specification<Patient>
{
  public PatientByIdSpec(int patientId) =>
    Query
        .Where(patient => patient.Id == patientId);
}
