namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

public class PatientByPatientIdSpec : Specification<Patient>
{
    public PatientByPatientIdSpec(PatientId patientId) =>
      Query
          .Where(patient => patient.PatientId == patientId);
}
