using PatientHealthRecord.Core.ValueObjects;

namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

public class PatientsByGenderSpec : Specification<Patient>
{
    public PatientsByGenderSpec(Gender gender) =>
      Query
          .Where(patient => patient.Gender == gender);
}
