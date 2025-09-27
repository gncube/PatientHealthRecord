namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

public class ActivePatientsSpec : Specification<Patient>
{
    public ActivePatientsSpec() =>
      Query
          .Where(patient => patient.IsActive);
}
