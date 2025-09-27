namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

public class PatientByEmailSpec : Specification<Patient>
{
    public PatientByEmailSpec(string email) =>
      Query
          .Where(patient => patient.Email == email);
}
